using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
            
            var user = await _serviceUser.FindByIdAsync(id.Value);
            
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _serviceUser.FindByIdAsync(id.Value);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTO dto)
        {
            if (id != dto.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                ModelState.AddModelError(nameof(dto.Username), "Debe ingresar el nombre de usuario.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Debe ingresar el correo electrónico.");
            }

            if (dto.UserState is null)
            {
                ModelState.AddModelError(nameof(dto.UserState), "Debe seleccionar el estado del usuario.");
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join("\n",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                ViewData["SwalError"] = JsonSerializer.Serialize(new
                {
                    title = "Errores de validación",
                    text = errores,
                    icon = "warning"
                });

                return View(dto);
            }

            await _serviceUser.UpdateAsync(dto);

            TempData["EditedUserId"] = dto.Id;
            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Usuario actualizado correctamente",
                text = "Los datos del usuario fueron actualizados.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
