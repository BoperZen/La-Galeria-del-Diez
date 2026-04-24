using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Authorize]
    public class PujasController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceBidding _serviceBidding;
        private readonly IServiceUser _serviceUser;

        public PujasController(IServiceAuction serviceAuction, IServiceBidding serviceBidding, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceBidding = serviceBidding;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? auctionId)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var targetId = auctionId ?? id ?? 0;
            if (targetId <= 0)
            {
                return RedirectToAction("Index", "Subasta");
            }

            var auction = await _serviceAuction.FindByIdAsync(targetId);
            if (auction == null)
            {
                return RedirectToAction("Index", "Subasta");
            }

            var currentPrice = auction.Biddings?.Any() == true
                ? auction.Biddings.Max(b => b.Amount)
                : auction.BasePrice;

            var viewModel = new BiddingCreateViewModel
            {
                AuctionId = auction.Id,
                AuctionName = auction.Object.Name,
                CurrentPrice = currentPrice,
                MinIncrement = auction.MinIncrement
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BiddingCreateViewModel model)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (model.AuctionId <= 0)
            {
                return RedirectToAction("Index", "Subasta");
            }

            var auction = await _serviceAuction.FindByIdAsync(model.AuctionId);
            if (auction == null)
            {
                return RedirectToAction("Index", "Subasta");
            }

            var currentPrice = auction.Biddings?.Any() == true
                ? auction.Biddings.Max(b => b.Amount)
                : auction.BasePrice;

            model.AuctionName = auction.Object.Name;
            model.CurrentPrice = currentPrice;
            model.MinIncrement = auction.MinIncrement;

            if (model.Amount <= 0)
            {
                ModelState.AddModelError(nameof(model.Amount), "El monto debe ser mayor que 0.");
            }

            if (string.IsNullOrWhiteSpace(model.PaymentMethod))
            {
                ModelState.AddModelError(nameof(model.PaymentMethod), "Debe indicar el método de pago.");
            }

            var user = await _serviceUser.FindByIdAsync(currentUserId);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var biddingDto = new BiddingDTO
            {
                IdAuction = model.AuctionId,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                IdUser = user.Id
            };

            try
            {
                await _serviceBidding.AddAsync(biddingDto);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", model);
            }

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Puja registrada",
                text = "Tu puja fue registrada correctamente.",
                icon = "success"
            });

            return RedirectToAction("Details", "Subasta", new { id = model.AuctionId });
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var currentUserId) ? currentUserId : 0;
        }
    }
}
