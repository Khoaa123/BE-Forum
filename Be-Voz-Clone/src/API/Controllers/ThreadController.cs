using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ThreadController : ControllerBase
{
    private readonly IThreadService _threadService;

    public ThreadController(IThreadService threadService)
    {
        _threadService = threadService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetThreadId(int id, int pageNumber = 1, int pageSize = 5)
    {
        var result = await _threadService.GetAsync(id, pageNumber, pageSize);
        return StatusCode((int)result.StatusCode, result);
    }

    // GET api/<ThreadController>/5
    [HttpGet("ThreadsByForum")]
    public async Task<IActionResult> GetThreadsByForum(int id, int pageNumber = 1, int pageSize = 5)
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

    [HttpPost("RecordViewedThread")]
    public async Task<IActionResult> RecordViewedThread(string userId, int threadId)
    {
        await _threadService.RecordThreadView(userId, threadId);
        return Ok("Đã xem thread");
    }

    //[HttpGet("GetViewedThreads")]
    //public async Task<IActionResult> GetViewedThreads(string userId, int limit = 10)
    //{
    //    var result = await _threadService.GetViewedThread(userId, limit);
    //    return StatusCode((int)result.StatusCode, result);
    //}

    [HttpGet("GetThreadByUserId")]
    public async Task<IActionResult> GetThreadByUserId(string userId)
    {
        var result = await _threadService.GetThreadsByUseridAsync(userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("ToggleSticky")]
    public async Task<IActionResult> ToggleSticky(int id)
    {
        var result = await _threadService.ToggleStickyAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }
}