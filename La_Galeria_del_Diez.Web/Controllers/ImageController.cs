using La_Galeria_del_Diez.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace La_Galeria_del_Diez.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IServiceObject _serviceObject;

        public ImageController(IServiceObject serviceObject)
        {
            _serviceObject = serviceObject;
        }

        // GET: api/Image/5
        [HttpGet("{objectId}")]
        [ResponseCache(Duration = 3600)] // Cache por 1 hora
        public async Task<IActionResult> GetImage(int objectId)
        {
            try
            {
                var obj = await _serviceObject.FindByIdAsync(objectId);
                
                if (obj?.Images == null || !obj.Images.Any())
                {
                    return NotFound();
                }

                var image = obj.Images.First();
                return File(image.Data, "image/jpeg");
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/Image/ByImageId/5
        [HttpGet("ByImageId/{imageId}")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> GetImageById(int imageId, [FromQuery] int objectId)
        {
            try
            {
                var obj = await _serviceObject.FindByIdAsync(objectId);
                
                if (obj?.Images == null || !obj.Images.Any())
                {
                    return NotFound();
                }

                var image = obj.Images.FirstOrDefault(i => i.Id == imageId);
                if (image == null)
                {
                    return NotFound();
                }

                return File(image.Data, "image/jpeg");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
