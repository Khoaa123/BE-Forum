using Be_Voz_Clone.src.Services;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Shared.Core.Helper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers;

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

    [HttpPost("Update-Badge")]
    public async Task<IActionResult> UpdateBadge()
    {
        await _accountService.UpdateBadgesAsync();
        return Ok("Badges updated successfully.");
    }

    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file, string userId)
    {
        var result = await _accountService.UploadAvatarUrlAsync(userId, file);

        return Ok(new { Message = "Avatar updated successfully!", AvatarUrl = result });
    }

    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser(string userId)
    {
        var result = await _accountService.GetUserAsync(userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetUser(int pageNumber = 1, int pageSize = 5)
    {
        var result = await _accountService.GetAllUserAsync(pageNumber, pageSize);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _accountService.DeleteUserAsync(userId);
        return StatusCode((int)result.StatusCode, result);
    }
}