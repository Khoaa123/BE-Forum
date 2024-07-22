using Be_Voz_Clone.src.Services.DTO.Account;

namespace Be_Voz_Clone.src.Services
{
    public interface IAccountService
    {
        Task<RegisterObjectResponse> RegisterAsync(AccountRegisterRequest request, string role);
        Task<TokenObjectResponse> LoginAsync(AccountLoginRequest request);
        Task<TokenObjectResponse> GetRefreshTokenAsync(TokenRequest request);
    }
}
