using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceUser.ListAsync();
            return View(collection);
        }
    }
}
