using La_Galeria_del_Diez.Application.DTOs.Api;
using La_Galeria_del_Diez.Application.DTOs.Api;
using La_Galeria_del_Diez.Application.Services.Interfaces.Api;
using Microsoft.AspNetCore.Mvc;

namespace La_Galeria_del_Diez.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionApiService _service;

    public AuctionController(IAuctionApiService service)
    {
        _service = service;
    }

    // GET: api/auction
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var list = await _service.ListAsync();
        return Ok(list);
    }

    // GET: api/auction/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var auction = await _service.FindByIdAsync(id);
        if (auction is null) return NotFound();

        return Ok(auction);
    }
}
