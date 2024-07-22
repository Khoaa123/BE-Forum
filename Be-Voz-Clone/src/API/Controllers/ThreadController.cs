using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadService _threadService;

        public ThreadController(IThreadService threadService)
        {
            _threadService = threadService;
        }

        // GET api/<ThreadController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, int pageNumber = 1, int pageSize = 5)
        {
            var result = await _threadService.GetThreadsByForumAsync(id, pageNumber, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }

        // POST api/<ThreadController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ThreadRequest request)
        {
            var result = await _threadService.CreateAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        // DELETE api/<ThreadController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _threadService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
