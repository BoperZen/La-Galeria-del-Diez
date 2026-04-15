using Microsoft.AspNetCore.Mvc;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Models;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private int IdUser = 5;

        public PaymentController(IServiceAuction serviceAuction)
        {
            _serviceAuction = serviceAuction;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(IdUser);

            return View(pendingAuctions);
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(IdUser);
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
            var auction = await _serviceAuction.FindByIdAsync(model.AuctionId);
            if (auction == null)
            {
                TempData["PaymentError"] = "Auction not found.";
                return RedirectToAction(nameof(Index));
            }

            var pendingAuctions = await _serviceAuction.ListPendingPaymentByWinnerAsync(IdUser);
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
    }
}
