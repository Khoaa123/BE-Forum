using Be_Voz_Clone.src.Shared.Core.Enums;

namespace Be_Voz_Clone.src.Services.DTO.Reaction
{
    public class ReactionRequest
    {
        public ReactionType Type { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
