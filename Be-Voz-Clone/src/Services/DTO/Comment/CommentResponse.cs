using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Services.DTO.Reaction;
using System.Text.Json.Serialization;

namespace Be_Voz_Clone.src.Services.DTO.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ThreadName { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ParentCommentId { get; set; }
        public List<CommentResponse> Replies { get; set; } = new List<CommentResponse>();
        public List<ReactionResponse> Reactions { get; set; } = new List<ReactionResponse>();
    }

    public class CommentObjectResponse : ObjectResponse<CommentResponse> { }

    public class CommentListObjectResponse : ObjectResponse<List<CommentResponse>> { }
}
