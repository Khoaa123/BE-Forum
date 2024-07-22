using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Forum;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        // GET api/<ForumController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _forumService.GetForumsByCategoryAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // POST api/<ForumController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ForumRequest request)
        {
            var result = await _forumService.CreateAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }


        // DELETE api/<ForumController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _forumService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
