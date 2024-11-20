using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Reaction;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // POST api/<CommentController>
    [HttpPost("Comment")]
    public async Task<IActionResult> PostComment([FromBody] CommentRequest request)
    {
        var result = await _commentService.CreateAsync(request);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("ReplyComment")]
    public async Task<IActionResult> ReplyComment([FromBody] CommentRequest request)
    {
        var result = await _commentService.ReplyAsync(request);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("Reaction")]
    public async Task<IActionResult> AddReaction([FromBody] ReactionRequest request)
    {
        var result = await _commentService.AddReactionAsync(request);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("GetAllComment")]
    public async Task<IActionResult> GetAllComment([FromQuery] string userId)
    {
        var result = await _commentService.GetAllCommentAsync(userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentId(int id)
    {
        var result = await _commentService.GetCommentId(id);
        return StatusCode((int)result.StatusCode, result);
    }
}