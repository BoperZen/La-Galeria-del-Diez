using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class ObjectController : Controller
    {
        private readonly IServiceObject _serviceObject;

        public ObjectController(IServiceObject serviceObject)
        {
            _serviceObject = serviceObject;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var collection = await _serviceObject.ListAsync();
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            var obj = await _serviceObject.FindByIdAsync(id.Value);
            
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
