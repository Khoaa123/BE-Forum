namespace Be_Voz_Clone.src.Services.DTO.Comment
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public int ThreadId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
