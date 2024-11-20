using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Services.DTO.User;

namespace Be_Voz_Clone.src.Services;

public interface IAccountService
{
    Task<RegisterObjectResponse> RegisterAsync(AccountRegisterRequest request, string role);
    Task<TokenObjectResponse> LoginAsync(AccountLoginRequest request);
    Task<TokenObjectResponse> GetRefreshTokenAsync(TokenRequest request);
    Task<UserObjectResponse> GetUserAsync(string userId);
    Task<string> UploadAvatarUrlAsync(string userId, IFormFile file);
    Task UpdateBadgesAsync();
}