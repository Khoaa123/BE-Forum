using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedThreadController : ControllerBase
    {
        private readonly ISavedThreadService _savedThreadService;

        public SavedThreadController(ISavedThreadService savedThreadService)
        {
            _savedThreadService = savedThreadService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSavedThreads(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _savedThreadService.GetSavedThreadsAsync(userId, pageNumber, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("SaveThread")]
        public async Task<IActionResult> SaveThread(string userId, int threadId)
        {
            await _savedThreadService.SaveThreadAsync(userId, threadId);
            return Ok("Đã lưu thread");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(string userId, int id)
        {
            await _savedThreadService.RemoveSavedThreadAsync(userId, id);
            return Ok("Xóa lưu thành công");
        }

        [HttpGet("Is-saved")]
        public async Task<IActionResult> IsThreadSaved(string userId, int threadId)
        {
            var result = await _savedThreadService.IsThreadSavedAsync(userId, threadId);
            return Ok(result);
        }

    }
}
