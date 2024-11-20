using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Services.DTO.Comment;

namespace Be_Voz_Clone.src.Services.DTO.Thread
{
    public class ThreadResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSticky { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tag { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public string UserId { get; set; }
        public string ForumName { get; set; }
        public int ForumId { get; set; }
        public int? TotalComments { get; set; }
        public List<CommentResponse>? Comments { get; set; }
        public DateTime? LastCommentAt { get; set; }
        public string LastCommentBy { get; set; }
        public string LastCommenterAvatarUrl { get; set; }
    }

    public class LatestThreadResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AvatarUrl { get; set; }
    }


    public class ThreadListObjectResponse : ObjectResponse<List<ThreadResponse>> { }
    public class ThreadObjectResponse : ObjectResponse<ThreadResponse> { }
}
