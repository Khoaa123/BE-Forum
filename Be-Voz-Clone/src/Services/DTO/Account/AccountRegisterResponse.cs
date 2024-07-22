using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.Account
{
    public class AccountRegisterResponse
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }

    public class RegisterObjectResponse : ObjectResponse<AccountRegisterResponse> { }
}
