using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ViewedThreadController : ControllerBase
{
    private readonly IViewedThreadService _viewedThreadService;

    public ViewedThreadController(IViewedThreadService viewedThreadService)
    {
        _viewedThreadService = viewedThreadService;
    }

    // GET api/<ViewedThreadController>/5
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string userId, int limit = 10)
    {
        var result = await _viewedThreadService.ViewedThreadListAsync(userId, limit);

        return StatusCode((int)result.StatusCode, result);
    }
}