using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using Libreria.Web.Util;
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
        private int IdUser = 1;

        public SubastaController(IServiceAuction serviceAuction, IServiceObject serviceObject, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceObject = serviceObject;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var activeAuctions = await _serviceAuction.ListActiveAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(activeAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Finalizadas(int? page)
        {
            var finishedAuctions = await _serviceAuction.ListFinishedAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(finishedAuctions.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Borrador(int? page)
        {
            var DraftAuctions = await _serviceAuction.ListDraftAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(DraftAuctions.ToPagedList(pageNumber, pageSize));
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
            await GetUser(IdUser);
            return View(new AuctionDTO());
        }

        //Create Auction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AuctionDTO dto)
        {
            if (dto.IdObject <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdObject), "You must select an object.");
            }

            if (dto.IdUser <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdUser), "You must select a user.");
            }

            if (dto.BasePrice <= 0)
            {
                ModelState.AddModelError(nameof(dto.BasePrice), "The base price must be greater than 0.");
            }

            if (dto.MinIncrement <= 0)
            {
                ModelState.AddModelError(nameof(dto.MinIncrement), "The minimum increment must be greater than 0.");
            }

            if (dto.StartDate == default)
            {
                ModelState.AddModelError(nameof(dto.StartDate), "You must enter a start date.");
            }

            if (dto.EndDate == default)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "You must enter an end date.");
            }

            if (dto.StartDate != default && dto.EndDate != default && dto.EndDate <= dto.StartDate)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "The end date must be after the start date.");
            }

            dto.IdState = 2;

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Validation errors",
                    text = errores,
                    icon = "warning"
                });

                await LoadCombosAsync();
                await GetUser(IdUser);
                return View(dto);
            }

            await _serviceAuction.AddAsync(dto);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction created successfully",
                text = "The auction was registered successfully.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        //Create Auction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBorrador(AuctionDTO dto)
        {
            if (dto.IdObject <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdObject), "You must select an object.");
            }

            if (dto.IdUser <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdUser), "You must select a user.");
            }

            if (dto.BasePrice <= 0)
            {
                ModelState.AddModelError(nameof(dto.BasePrice), "The base price must be greater than 0.");
            }

            if (dto.MinIncrement <= 0)
            {
                ModelState.AddModelError(nameof(dto.MinIncrement), "The minimum increment must be greater than 0.");
            }

            if (dto.StartDate == default)
            {
                ModelState.AddModelError(nameof(dto.StartDate), "You must enter a start date.");
            }

            if (dto.EndDate == default)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "You must enter an end date.");
            }

            if (dto.StartDate != default && dto.EndDate != default && dto.EndDate <= dto.StartDate)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "The end date must be after the start date.");
            }

            dto.IdState = 1; // Estado "Borrador"

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Validation errors",
                    text = errores,
                    icon = "warning"
                });

                await LoadCombosAsync();
                await GetUser(IdUser);
                return View(dto);
            }

            await _serviceAuction.AddAsync(dto);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction created successfully",
                text = "The auction was registered successfully.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: LibroController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var dto = await _serviceAuction.FindByIdAsync(id.Value);
            DateTime now = DateTime.Now;
            if (now < dto.StartDate)
            {
                ViewBag.Object = await _serviceObject.FindByIdAsync(dto.IdObject);
                ViewBag.Auctionable_ObjectDTO = ViewBag.Object;
                ViewBag.User = await _serviceUser.FindByIdAsync(dto.IdUser);
                return View(dto);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AuctionDTO dto)
        {
            if (dto.BasePrice <= 0)
            {
                ModelState.AddModelError(nameof(dto.BasePrice), "The base price must be greater than 0.");
            }

            if (dto.MinIncrement <= 0)
            {
                ModelState.AddModelError(nameof(dto.MinIncrement), "The minimum increment must be greater than 0.");
            }

            if (dto.StartDate == default)
            {
                ModelState.AddModelError(nameof(dto.StartDate), "You must enter a start date.");
            }

            if (dto.EndDate == default)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "You must enter an end date.");
            }

            if (dto.StartDate != default && dto.EndDate != default && dto.EndDate <= dto.StartDate)
            {
                ModelState.AddModelError(nameof(dto.EndDate), "The end date must be after the start date.");
            }

            if (!ModelState.IsValid)
            {
                // Recopilar todos los errores del ModelState
                var errores = string.Join("<br>",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Validation errors",
                    text = errores,
                    icon = "warning"
                });
                ViewBag.Object = await _serviceObject.FindByIdAsync(dto.IdObject);
                ViewBag.Auctionable_ObjectDTO = ViewBag.Object;
                ViewBag.User = await _serviceUser.FindByIdAsync(dto.IdUser);
                return View(dto);
            }

            await _serviceAuction.UpdateAsync(id, dto);
            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction updated",
                text = "The auction was updated successfully.",
                icon = "success"
            });
            return RedirectToAction(nameof(Borrador));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish(int id)
        {
            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Borrador));
            }

            await _serviceAuction.UpdateStateAsync(id, 2);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction published",
                text = "The auction is now active.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Borrador));
            }

            await _serviceAuction.UpdateStateAsync(id, 1);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction delete",
                text = "The auction is now inactive.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
