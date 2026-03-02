using La_Galeria_del_Diez.Application.Services.Interfaces;
using Libreria.Web.Util;
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
            //Paginado
            int pageNumber = page ?? 1;
            int pageSize = 5;


            //Cantidad de elementos por página
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Notification"] = SweetAlertHelper.CrearNotificacion(
                       "Object not found",
                       $"There is no Object without an ID",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }
                var @object = await _serviceObject.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Object not Found");

                }
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "User Details",
                   $"Showing User Information: {@object.Name}",
                   SweetAlertMessageType.info
               );
                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
