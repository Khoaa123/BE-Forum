using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmojiAndStickerController : ControllerBase
    {
        private readonly IEmojiAndStickerService _emojiAndStickerService;

        public EmojiAndStickerController(IEmojiAndStickerService emojiAndStickerService)
        {
            _emojiAndStickerService = emojiAndStickerService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            var result = await _emojiAndStickerService.GetUrl(name);
            return Ok(result);
        }
    }
}