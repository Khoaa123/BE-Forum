using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Shared.Core.Enums;

namespace Be_Voz_Clone.src.Services.DTO.Reaction
{
    public class ReactionResponse
    {
        public int Id { get; set; }
        public ReactionType Type { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReactionObjectResponse : ObjectResponse<ReactionResponse> { }
}
