using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.Account
{
    public class AccountResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public string Role { get; set; }
        public DateTime JoinedDate { get; set; }
        public bool IsBanned { get; set; }
    }

    public class AccountObjectResponse : ObjectResponse<AccountResponse> { }

    public class AccountListObjectResponse : ObjectResponse<List<AccountResponse>> { }
}
