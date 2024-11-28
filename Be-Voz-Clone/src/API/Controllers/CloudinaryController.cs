using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CloudinaryController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public CloudinaryController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    // POST api/<CloudinaryController>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImages(List<IFormFile> files, string folderName, string name)
    {
        var urls = await _cloudinaryService.UploadImagesAsync(files, folderName, name);
        return Ok(new { urls });
    }
}