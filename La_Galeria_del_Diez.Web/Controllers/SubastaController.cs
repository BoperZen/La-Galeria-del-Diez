using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class SubastaController : Controller
    {
        private readonly IServiceObject _serviceObject;

        public SubastaController(IServiceObject serviceObject)
        {
            _serviceObject = serviceObject;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var collection = await _serviceObject.ListAsync();
            
            // Filtrar solo objetos con subastas activas (fecha no vencida)
            var activeAuctions = collection
                .Where(o => o.Auctions != null && 
                           o.Auctions.Any(a => a.EndDate > DateTime.Now))
                .ToList();
            
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(activeAuctions.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Finalizadas(int? page)
        {
            var collection = await _serviceObject.ListAsync();
            
            // Filtrar solo objetos con subastas finalizadas
            var finishedAuctions = collection
                .Where(o => o.Auctions != null && o.Auctions.Any(a => a.EndDate <= DateTime.Now))
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
            
            // Buscar todos los objetos
            var allObjects = await _serviceObject.ListAsync();
            
            // Buscar el objeto que contiene la subasta con el ID especificado
            var objectWithAuction = allObjects
                .FirstOrDefault(o => o.Auctions != null && o.Auctions.Any(a => a.Id == id.Value));
            
            if (objectWithAuction == null)
            {
                return RedirectToAction("Index");
            }
            
            // Obtener la subasta específica
            var auction = objectWithAuction.Auctions.FirstOrDefault(a => a.Id == id.Value);
            
            if (auction == null)
            {
                return RedirectToAction("Index");
            }
            
            // Pasar tanto el objeto como la subasta a la vista
            ViewBag.AuctionObject = objectWithAuction;
            return View(auction);
        }
    }
}
