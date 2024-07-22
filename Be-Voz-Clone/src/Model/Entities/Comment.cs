namespace Be_Voz_Clone.src.Model.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ThreadId { get; set; }
        public VozThread Thread { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public List<string>? Reaction { get; set; }
    }
}
