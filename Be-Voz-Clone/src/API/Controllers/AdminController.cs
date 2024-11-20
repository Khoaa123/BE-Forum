using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 5)
        {
            var result = await _adminService.GetAllUserAsync(pageNumber, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("BanUser/{userId}")]
        public async Task<IActionResult> BanUser(string userId)
        {
            var result = await _adminService.BanUserAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("UnbanUser/{userId}")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var result = await _adminService.UnbanUserAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}