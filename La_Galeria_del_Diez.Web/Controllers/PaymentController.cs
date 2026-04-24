using Microsoft.AspNetCore.Mvc;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceUser _serviceUser;

        public PaymentController(IServiceAuction serviceAuction, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accessResult = await EnsurePaymentAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var currentUserId = GetCurrentUserId();

            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(currentUserId);

            return View(pendingAuctions);
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var accessResult = await EnsurePaymentAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var currentUserId = GetCurrentUserId();

            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(currentUserId);
            var isPayable = pendingAuctions.Any(a => a.Id == id);
            if (!isPayable)
            {
                TempData["PaymentError"] = "This auction is not available for payment.";
                return RedirectToAction(nameof(Index));
            }

            var model = new PaymentProcessViewModel
            {
                AuctionId = auction.Id,
                AuctionName = auction.Object?.Name ?? $"Auction #{auction.Id}"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(PaymentProcessViewModel model)
        {
            var accessResult = await EnsurePaymentAccessAsync();
            if (accessResult != null)
            {
                return accessResult;
            }

            var currentUserId = GetCurrentUserId();

            var auction = await _serviceAuction.FindByIdAsync(model.AuctionId);
            if (auction == null)
            {
                TempData["PaymentError"] = "Auction not found.";
                return RedirectToAction(nameof(Index));
            }

            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(currentUserId);
            var isPayable = pendingAuctions.Any(a => a.Id == model.AuctionId);
            if (!isPayable)
            {
                TempData["PaymentError"] = "This auction is not available for payment.";
                return RedirectToAction(nameof(Index));
            }

            model.AuctionName = auction.Object?.Name ?? $"Auction #{auction.Id}";

            var now = DateTime.UtcNow;
            if (model.ExpirationYear < now.Year ||
                (model.ExpirationYear == now.Year && model.ExpirationMonth < now.Month))
            {
                ModelState.AddModelError(nameof(model.ExpirationMonth), "Card is expired.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _serviceAuction.UpdateStateAsync(model.AuctionId, 4);

            TempData["PaymentSuccess"] = "Payment processed successfully.";
            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var currentUserId) ? currentUserId : 0;
        }

        private async Task<IActionResult?> EnsurePaymentAccessAsync()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId <= 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = await _serviceUser.FindByIdAsync(currentUserId);
            if (currentUser?.IdRol is not 1 and not 3)
            {
                return RedirectToAction("Forbidden", "Login");
            }

            return null;
        }
    }
}
