using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Authorize]
    public class ObjectController : Controller
    {
        private readonly IServiceObject _serviceObject;
        private readonly IServiceUser _serviceUser;

        public ObjectController(IServiceObject serviceObject, IServiceUser serviceUser)
        {
            _serviceObject = serviceObject;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var collection = await _serviceObject.ListAsync();
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var obj = await _serviceObject.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var dto = new Auctionable_ObjectDTO
            {
                RegistrationDate = DateTime.Now,
                IdState = 6
            };

            var seller = await GetCurrentManagerAsync();
            if (seller != null)
            {
                dto.IdUser = seller.Id;
            }

            await LoadFormDataAsync(dto);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Auctionable_ObjectDTO dto, int[] selectedCategoryIds, List<IFormFile>? imageFiles)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            try
            {
                var seller = await GetCurrentManagerAsync();
                if (seller != null)
                {
                    dto.IdUser = seller.Id;
                }

                dto.IdState = 6;
                dto.Categories = selectedCategoryIds.Select(id => new CategoryDTO { Id = id }).ToList();
                dto.Images = await MapImagesAsync(imageFiles);

                await _serviceObject.AddAsync(dto);

                TempData["SwalSuccess"] = JsonSerializer.Serialize(new
                {
                    title = "Objeto creado correctamente",
                    text = "El objeto fue registrado exitosamente.",
                    icon = "success"
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            await LoadFormDataAsync(dto, selectedCategoryIds);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = await _serviceObject.FindByIdAsync(id.Value);
            if (dto == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!await _serviceObject.CanEditAsync(id.Value))
            {
                TempData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Edición no permitida",
                    text = "El objeto no se puede editar porque está en subasta activa o su estado no lo permite.",
                    icon = "warning"
                });
                return RedirectToAction(nameof(Index));
            }

            await LoadFormDataAsync(dto, dto.Categories.Select(c => c.Id));
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Auctionable_ObjectDTO dto, int[] selectedCategoryIds, List<IFormFile>? imageFiles)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            if (id != dto.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var existing = await _serviceObject.FindByIdAsync(id);
                if (existing == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                dto.IdUser = existing.IdUser;
                dto.IdState = existing.IdState;
                dto.RegistrationDate = existing.RegistrationDate;
                dto.Categories = selectedCategoryIds.Select(categoryId => new CategoryDTO { Id = categoryId }).ToList();
                dto.Images = await MapImagesAsync(imageFiles);

                await _serviceObject.UpdateAsync(dto);

                TempData["SwalSuccess"] = JsonSerializer.Serialize(new
                {
                    title = "Objeto actualizado correctamente",
                    text = "Los cambios del objeto fueron guardados.",
                    icon = "success"
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            await LoadFormDataAsync(dto, selectedCategoryIds);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var accessResult = await EnsureObjectManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            try
            {
                await _serviceObject.DeleteAsync(id);

                TempData["SwalSuccess"] = JsonSerializer.Serialize(new
                {
                    title = "Objeto cancelado",
                    text = "El objeto fue marcado como cancelado.",
                    icon = "success"
                });
            }
            catch (InvalidOperationException ex)
            {
                TempData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "No se puede eliminar",
                    text = ex.Message,
                    icon = "warning"
                });
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadFormDataAsync(Auctionable_ObjectDTO dto, IEnumerable<int>? selectedCategoryIds = null)
        {
            var allObjects = await _serviceObject.ListAsync();
            var categories = allObjects
                .SelectMany(o => o.Categories)
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .OrderBy(c => c.Description)
                .ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryIds = selectedCategoryIds?.ToHashSet() ?? new HashSet<int>();

            var seller = await GetCurrentManagerAsync();
            if (seller != null)
            {
                dto.IdUser = seller.Id;
                ViewBag.Seller = seller;
            }
        }

        private static async Task<List<ImageDTO>> MapImagesAsync(List<IFormFile>? imageFiles)
        {
            var images = new List<ImageDTO>();

            if (imageFiles == null || imageFiles.Count == 0)
            {
                return images;
            }

            foreach (var file in imageFiles.Where(f => f.Length > 0))
            {
                using var memory = new MemoryStream();
                await file.CopyToAsync(memory);

                images.Add(new ImageDTO
                {
                    Data = memory.ToArray(),
                    RegistrationDate = DateTime.Now
                });
            }

            return images;
        }

        private async Task<IActionResult?> EnsureObjectManagerAccessAsync()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var currentUserId) || currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = await _serviceUser.FindByIdAsync(currentUserId);
            if (currentUser?.IdRol is not 1 and not 2)
            {
                return RedirectToAction("Forbidden", "Login");
            }

            return null;
        }

        private async Task<UserDTO?> GetCurrentManagerAsync()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var currentUserId) || currentUserId <= 0)
            {
                return null;
            }

            var currentUser = await _serviceUser.FindByIdAsync(currentUserId);
            return currentUser?.IdRol is 1 or 2 ? currentUser : null;
        }
    }
}
