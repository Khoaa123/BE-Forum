using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Shared.Core.Helper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest request)
        {
            var result = await _accountService.LoginAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AccountRegisterRequest request)
        {
            var result = await _accountService.RegisterAsync(request, Roles.Admin);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("RegisterMod")]
        public async Task<IActionResult> RegisterDrugstore([FromBody] AccountRegisterRequest request)
        {
            var result = await _accountService.RegisterAsync(request, Roles.Mod);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] AccountRegisterRequest request)
        {
            var result = await _accountService.RegisterAsync(request, Roles.User);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            var result = await _accountService.GetRefreshTokenAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
