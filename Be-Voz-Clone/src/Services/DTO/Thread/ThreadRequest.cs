namespace Be_Voz_Clone.src.Services.DTO.Thread
{
    public class ThreadRequest
    {
        public string Title { get; set; }
        public bool IsSticky { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tag { get; set; }
        public string UserId { get; set; }
        public int ForumId { get; set; }
    }
}
