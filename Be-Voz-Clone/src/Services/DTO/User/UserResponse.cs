using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.User
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public int ReactionScore { get; set; }
        public string LastActivity { get; set; }
    }

    public class UserObjectResponse : ObjectResponse<UserResponse> { }
}
