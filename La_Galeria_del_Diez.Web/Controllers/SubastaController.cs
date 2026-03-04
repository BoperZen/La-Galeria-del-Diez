using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class SubastaController : Controller
    {
        private readonly IServiceAuction _serviceAuction;

        public SubastaController(IServiceAuction serviceAuction)
        {
            _serviceAuction = serviceAuction;
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
    }
}
