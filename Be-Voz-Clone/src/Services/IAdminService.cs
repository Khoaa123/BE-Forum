using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Services.DTO.Account;

namespace Be_Voz_Clone.src.Services
{
    public interface IAdminService
    {
        Task<AccountListObjectResponse> GetAllUserAsync(int pageNumber, int pageSize);
        Task<BaseResponse> BanUserAsync(string userId);
        Task<BaseResponse> UnbanUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(string userId, string newRole);

    }
}
