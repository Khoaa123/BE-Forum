using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudinaryController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public CloudinaryController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        // GET: api/<CloudinaryController>
        //[HttpGet("Emoji")]
        //public async Task<IActionResult> GetAllUrls()
        //{
        //    var folder = "Voz-Emoji";
        //    var urls = await _cloudinaryService.GetImageLinksAsync(folder);

        //    return Ok(new { urls = urls });

        //}

        //// GET api/<CloudinaryController>/5
        //[HttpGet("{folder}")]
        //public async Task<IActionResult> GetAllUrls(string folder)
        //{
        //    var urls = await _cloudinaryService.GetImageLinksAsync(folder);
        //    return Ok(new { urls = urls });
        //}

        // POST api/<CloudinaryController>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files, string folderName, string name)
        {
            var urls = await _cloudinaryService.UploadImagesAsync(files, folderName, name);

            return Ok(new { urls });
        }


        // PUT api/<CloudinaryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CloudinaryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
