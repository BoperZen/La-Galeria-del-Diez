using La_Galeria_del_Diez.Application.Services.Interfaces;
using Libreria.Web.Util;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public UserController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var collection = await _serviceUser.ListAsync();
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
                    TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                       "User not found",
                       $"There is no User without an ID",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }
                var @object = await _serviceUser.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("User not Found");

                }
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "User Details",
                   $"Showing User Information: {@object.Username}",
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
