using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Globalization;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Authorize]
    public class SubastaController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceBidding _serviceBidding;
        private readonly IServiceObject _serviceObject;
        private readonly IServiceUser _serviceUser;
        private readonly IHubContext<AuctionHub> _auctionHub;

        public SubastaController(IServiceAuction serviceAuction, IServiceBidding serviceBidding, IServiceObject serviceObject, IServiceUser serviceUser, IHubContext<AuctionHub> auctionHub)
        {
            _serviceAuction = serviceAuction;
            _serviceBidding = serviceBidding;
            _serviceObject = serviceObject;
            _serviceUser = serviceUser;
            _auctionHub = auctionHub;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page)
        {
            var activeAuctions = await _serviceAuction.ListActiveAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(activeAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Finalizadas(int? page)
        {
            var finishedAuctions = await _serviceAuction.ListFinishedAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(finishedAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Borrador(int? page)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var DraftAuctions = await _serviceAuction.ListDraftAsync(DateTime.Now);

            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(DraftAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AllowAnonymous]
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

            var currentUserId = GetCurrentUserId();
            ViewBag.CurrentUserId = currentUserId;
            var currentUser = currentUserId > 0 ? await _serviceUser.FindByIdAsync(currentUserId) : null;
            ViewBag.CurrentUserRoleId = currentUser?.IdRol ?? 0;

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

        public async Task<IActionResult> Create()
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            await LoadCombosAsync();
            await GetUser(GetCurrentUserId());
            return View(new AuctionDTO());
        }

        //Create Auction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionDTO dto)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

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
                await GetUser(GetCurrentUserId());
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
        public async Task<IActionResult> CreateBorrador(AuctionDTO dto)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

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
                await GetUser(GetCurrentUserId());
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
        public async Task<IActionResult> Edit(int? id)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

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
        public async Task<IActionResult> Edit(int id, AuctionDTO dto)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
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
        public async Task<IActionResult> Publish(int id)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

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
        public async Task<IActionResult> Bid(int id, string amount)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Unauthorized(new { message = "You must sign in to place a bid." });
                }

                return RedirectToAction("Index", "Login");
            }

            if (!decimal.TryParse(amount, NumberStyles.Number, CultureInfo.InvariantCulture, out var bidAmount))
            {
                var invalidAmountMessage = "The bid amount is not valid.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { message = invalidAmountMessage });
                }

                TempData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Invalid bid",
                    text = invalidAmountMessage,
                    icon = "warning"
                });
                return RedirectToAction(nameof(Details), new { id });
            }

            var dto = new BiddingDTO
            {
                IdAuction = id,
                IdUser = currentUserId,
                Amount = bidAmount,
                PaymentMethod = "Null"
            };

            try
            {
                await _serviceBidding.AddAsync(dto);

                var auction = await _serviceAuction.FindByIdAsync(id);
                if (auction != null)
                {
                    var currentPrice = auction.Biddings?.Any() == true
                        ? auction.Biddings.Max(b => b.Amount)
                        : auction.BasePrice;

                    var leadingBid = auction.Biddings?
                        .OrderByDescending(b => b.Amount)
                        .ThenByDescending(b => b.RegistrationDate)
                        .FirstOrDefault();

                    var bidHistory = auction.Biddings?
                        .OrderByDescending(b => b.RegistrationDate)
                        .Select(b => new
                        {
                            idUser = b.IdUser,
                            userName = b.UserName,
                            amount = b.Amount,
                            registrationDate = b.RegistrationDate.ToString("dd/MM/yyyy HH:mm:ss")
                        })
                        .Cast<object>()
                        .ToList();

                    await _auctionHub.Clients.Group($"auction-{id}").SendAsync("BidUpdated", new
                    {
                        auctionId = id,
                        totalBids = auction.Biddings?.Count ?? 0,
                        currentPrice,
                        leaderUserId = leadingBid?.IdUser ?? 0,
                        leaderUser = !string.IsNullOrWhiteSpace(leadingBid?.UserName)
                            ? leadingBid!.UserName
                            : (leadingBid != null ? $"User #{leadingBid.IdUser}" : "No bids yet"),
                        history = bidHistory ?? new List<object>()
                    });
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Ok(new { success = true });
                }

                TempData["SwalSuccess"] = JsonSerializer.Serialize(new
                {
                    title = "Bid placed",
                    text = "Your bid was registered successfully.",
                    icon = "success"
                });
            }
            catch (InvalidOperationException ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { message = ex.Message });
                }

                TempData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Bid rejected",
                    text = ex.Message,
                    icon = "warning"
                });
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var accessResult = await EnsureAuctionManagerAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Borrador));
            }

            await _serviceAuction.UpdateStateAsync(id, 4);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Auction delete",
                text = "The auction is now inactive.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var currentUserId) ? currentUserId : 0;
        }

        private async Task<IActionResult?> EnsureAuctionManagerAccessAsync()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
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
    }
}
