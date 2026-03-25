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
        public IActionResult Create()
        {
            return View(new UserDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                ModelState.AddModelError(nameof(dto.Username), "Debe ingresar el nombre de usuario.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Debe ingresar el correo electrónico.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                ModelState.AddModelError(nameof(dto.Password), "Debe ingresar la contraseña.");
            }

            if (dto.IdRol <= 0)
            {
                ModelState.AddModelError(nameof(dto.IdRol), "Debe seleccionar un rol.");
            }

            if (dto.RegistrationDate == default)
            {
                dto.RegistrationDate = DateTime.Now;
            }

            if (dto.UserState is null)
            {
                dto.UserState = true;
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

            await _serviceUser.AddAsync(dto);

            TempData["SwalSuccess"] = JsonSerializer.Serialize(new
            {
                title = "Usuario creado correctamente",
                text = "El usuario fue registrado exitosamente.",
                icon = "success"
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
