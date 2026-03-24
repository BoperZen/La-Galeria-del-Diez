using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class SubastaController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceObject _serviceObject;
        private readonly IServiceUser _serviceUser;

        public SubastaController(IServiceAuction serviceAuction, IServiceObject serviceObject, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceObject = serviceObject;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var collection = await _serviceAuction.ListAsync();

            // Filtrar solo subastas activas (fecha no vencida)
            var activeAuctions = collection
                .Where(a => a.EndDate > DateTime.Now)
                .ToList();

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(activeAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Finalizadas(int? page)
        {
            var collection = await _serviceAuction.ListAsync();

            // Filtrar solo subastas finalizadas
            var finishedAuctions = collection
                .Where(a => a.EndDate <= DateTime.Now)
                .ToList();

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(finishedAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var auction = await _serviceAuction.FindByIdAsync(id.Value);

            if (auction == null)
            {
                return RedirectToAction("Index");
            }

            return View(auction);
        }

        private async Task LoadCombosAsync(IEnumerable<string>? selectedCategoriaIds = null)
        {
            // Objetos no subastados
            ViewBag.ListObject = await _serviceObject.ListAsyncNoAuction();
        }

        public async Task GetUser(int id)
        {
            ViewBag.User = await _serviceUser.FindByIdAsync(id);
        }

        public async Task<ActionResult> Create()
        {
            await LoadCombosAsync();
            await GetUser(1);
            return View(new AuctionDTO());
        }

        //Create Auction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AuctionDTO dto)
        {
            if (dto.IdObject <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdObject), "Debe seleccionar un objeto.");
            }

            if (dto.IdUser <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdUser), "Debe seleccionar un usuario.");
            }

            if (dto.BasePrice <= 0)
            {
                ModelState.AddModelError(nameof(dto.BasePrice), "El precio base debe ser mayor a 0.");
            }

            if (dto.MinIncrement <= 0)
            {
                ModelState.AddModelError(nameof(dto.MinIncrement), "El incremento mínimo debe ser mayor a 0.");
            }

            if (dto.StartDate == default)
            {
                ModelState.AddModelError(nameof(dto.StartDate), "Debe ingresar la fecha de inicio.");
            }

            if (dto.EndDate == default)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "Debe ingresar la fecha de cierre.");
            }

            if (dto.StartDate != default && dto.EndDate != default && dto.EndDate <= dto.StartDate)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "La fecha de cierre debe ser posterior a la fecha de inicio.");
            }

            if (dto.IdState <= 0)
            {
                dto.IdState = 2;
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Errores de validación",
                    text = errores,
                    icon = "warning"
                });

                await LoadCombosAsync();
                await GetUser(1);
                return View(dto);
            }

            await _serviceAuction.AddAsync(dto);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Subasta creada correctamente",
                text = "La subasta fue registrada exitosamente.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
