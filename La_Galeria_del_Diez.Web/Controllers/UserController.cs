using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public UserController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var accessDeniedResult = await EnsureAdminAccessAsync();
            if (accessDeniedResult != null)
            {
                return accessDeniedResult;
            }

            var collection = await _serviceUser.ListAsync();
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var requestedUserId = id ?? currentUserId;
            var currentUser = await _serviceUser.FindByIdAsync(currentUserId);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var isAdmin = currentUser.IdRol == 1;
            if (!isAdmin && requestedUserId != currentUserId)
            {
                return RedirectToAction("Forbidden", "Login");
            }

            var user = await _serviceUser.FindByIdAsync(requestedUserId);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var accessDeniedResult = await EnsureAdminAccessAsync();
            if (accessDeniedResult != null)
            {
                return accessDeniedResult;
            }

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _serviceUser.FindByIdAsync(id.Value);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTO dto)
        {
            var accessDeniedResult = await EnsureAdminAccessAsync();
            if (accessDeniedResult != null)
            {
                return accessDeniedResult;
            }

            if (id != dto.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                ModelState.AddModelError(nameof(dto.Username), "Debe ingresar el nombre de usuario.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Debe ingresar el correo electrónico.");
            }

            if (dto.UserState is null)
            {
                ModelState.AddModelError(nameof(dto.UserState), "Debe seleccionar el estado del usuario.");
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Errores de validación",
                    text = errores,
                    icon = "warning"
                });

                return View(dto);
            }

            await _serviceUser.UpdateAsync(dto);

            TempData["EditedUserId"] = dto.Id;
            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Usuario actualizado correctamente",
                text = "Los datos del usuario fueron actualizados.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var accessDeniedResult = await EnsureAdminAccessAsync();
            if (accessDeniedResult != null)
            {
                return accessDeniedResult;
            }

            return View(new UserDTO
            {
                IdRol = 3,
                UserState = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            var accessDeniedResult = await EnsureAdminAccessAsync();
            if (accessDeniedResult != null)
            {
                return accessDeniedResult;
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                ModelState.AddModelError(nameof(dto.Username), "Debe ingresar el nombre de usuario.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Debe ingresar el correo electrónico.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                ModelState.AddModelError(nameof(dto.Password), "Debe ingresar la contraseña.");
            }

            if (dto.IdRol <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdRol), "Debe seleccionar un rol.");
            }

            if (dto.UserState is null)
            {
                ModelState.AddModelError(nameof(dto.UserState), "Debe seleccionar el estado del usuario.");
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Errores de validación",
                    text = errores,
                    icon = "warning"
                });

                return View(dto);
            }

            await _serviceUser.AddAsync(dto);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Usuario creado",
                text = "El usuario fue registrado correctamente.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var currentUserId) ? currentUserId : 0;
        }

        private async Task<IActionResult?> EnsureAdminAccessAsync()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = await _serviceUser.FindByIdAsync(currentUserId);
            if (currentUser?.IdRol != 1)
            {
                return RedirectToAction("Forbidden", "Login");
            }

            return null;
        }
    }
}
