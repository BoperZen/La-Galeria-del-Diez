using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Application.DTOs;
using Libreria.Web.Util;
using Libreria.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace La_Galeria_del_Diez.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceUser _serviceUsuario;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IServiceUser serviceUsuario, ILogger<LoginController> logger)
        {
            _serviceUsuario = serviceUsuario;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(ViewModelLogin viewModelLogin)
        {
            if (!ModelState.IsValid)
            {
                string errores = string.Join("<br>", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                        ? "Unspecified validation error"
                        : e.ErrorMessage));

                ViewData["SwalError"] = SweetAlertHelper.CrearNotificacion(
                    "Validation errors",
                    $"The form contains the following errors:<br>{errores}",
                    SweetAlertMessageType.warning
                );

                _logger.LogWarning("Validation error in login for user {Usuario}. Details: {Errores}",
                    viewModelLogin.User, errores);

                return View("Index", viewModelLogin);
            }

            var usuarioLog = await _serviceUsuario.LoginAsync(viewModelLogin.User, viewModelLogin.Password);

            if (usuarioLog == null)
            {
                ViewData["SwalError"] = SweetAlertHelper.CrearNotificacion(
                    "Access denied",
                    "Incorrect username or password.",
                    SweetAlertMessageType.warning
                );

                _logger.LogWarning("Failed login attempt for user {Usuario}", viewModelLogin.User);

                return View("Index", viewModelLogin);
            }

            if (usuarioLog.UserState != true)
            {
                ViewData["SwalError"] = SweetAlertHelper.CrearNotificacion(
                    "Access denied",
                    "Your account is inactive.",
                    SweetAlertMessageType.warning
                );

                _logger.LogWarning("Inactive account login attempt for user {Usuario}", viewModelLogin.User);

                return View("Index", viewModelLogin);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{usuarioLog.Username}"),
                new Claim(ClaimTypes.Role, usuarioLog.IdRolNavigation.Description),
                new Claim(ClaimTypes.NameIdentifier, usuarioLog.Id.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            _logger.LogInformation("Successful login for user {Usuario}", viewModelLogin.User);

            TempData["SwalSuccess"] = SweetAlertHelper.CrearNotificacion(
                "Welcome",
                $"Successful login. Hello, {usuarioLog.Username}.",
                SweetAlertMessageType.success
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ViewModelRegisterUser model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            if (model.IdRol != 2 && model.IdRol != 3)
            {
                ModelState.AddModelError(nameof(model.IdRol), "Only buyer or seller accounts are allowed.");
            }

            if (!ModelState.IsValid)
            {
                string errores = string.Join("\n", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["SwalError"] = SweetAlertHelper.CrearNotificacion(
                    "Invalid registration",
                    errores,
                    SweetAlertMessageType.warning
                );

                return RedirectToAction(nameof(Index));
            }

            var dto = new UserDTO
            {
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                Password = model.Password,
                IdRol = model.IdRol,
                UserState = true,
                RegistrationDate = DateTime.Now
            };

            await _serviceUsuario.AddAsync(dto);

            TempData["SwalSuccess"] = SweetAlertHelper.CrearNotificacion(
                "Account created",
                "Your user was registered successfully. You can now sign in.",
                SweetAlertMessageType.success
            );

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            _logger.LogInformation("Successful sign-out for user {Usuario}", User.Identity?.Name);

            await HttpContext.SignOutAsync();

            TempData["SwalSuccess"] = SweetAlertHelper.CrearNotificacion(
                "Session ended",
                "You have successfully signed out.",
                SweetAlertMessageType.success
            );

            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
