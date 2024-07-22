namespace Be_Voz_Clone.src.Model.Entities
{
    public class VozThread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSticky { get; set; } // Đánh dấu Thread có được ghim lên đầu không.
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ViewCount { get; set; }
        public string Tag { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ForumId { get; set; }
        public Forum Forum { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
