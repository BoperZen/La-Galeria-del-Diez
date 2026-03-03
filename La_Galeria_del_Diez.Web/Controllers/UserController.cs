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
            int pageSize = 10;

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
                       "Usuario no encontrado",
                       "No se proporcionó un ID de usuario",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }
                
                var user = await _serviceUser.FindByIdAsync(id.Value);
                
                if (user == null)
                {
                    TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                       "Usuario no encontrado",
                       $"No existe un usuario con el ID: {id}",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }
                
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "Detalle de Usuario",
                   $"Mostrando información de: {user.Username}",
                   SweetAlertMessageType.info
               );
               
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                   "Error",
                   $"Ocurrió un error al cargar los detalles: {ex.Message}",
                   SweetAlertMessageType.error
               );
                return RedirectToAction("Index");
            }
        }
    }
}
