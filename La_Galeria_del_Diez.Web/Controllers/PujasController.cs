using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class PujasController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceBidding _serviceBidding;
        private readonly IServiceUser _serviceUser;
        private int _usuarioIdActualId = 1;

        public PujasController(IServiceAuction serviceAuction, IServiceBidding serviceBidding, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceBidding = serviceBidding;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? auctionId)
        {
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

            var user = await _serviceUser.FindByIdAsync(_usuarioIdActualId);
            if (user == null)
            {
                if (string.IsNullOrWhiteSpace(model.BuyerName))
                {
                    ModelState.AddModelError(nameof(model.BuyerName), "Debe ingresar el nombre del comprador.");
                }

                if (string.IsNullOrWhiteSpace(model.BuyerEmail))
                {
                    ModelState.AddModelError(nameof(model.BuyerEmail), "Debe ingresar el correo del comprador.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (user == null)
            {
                var newUser = new UserDTO
                {
                    Username = model.BuyerName.Trim(),
                    Email = model.BuyerEmail.Trim(),
                    Password = "Temporal123",
                    IdRol = 3,
                    UserState = true
                };

                user = await _serviceUser.AddAsync(newUser);
                _usuarioIdActualId = user.Id;
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
    }
}
