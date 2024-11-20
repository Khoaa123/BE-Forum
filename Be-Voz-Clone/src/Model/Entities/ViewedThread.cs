namespace Be_Voz_Clone.src.Model.Entities
{
    public class ViewedThread
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ThreadId { get; set; }
        public VozThread Thread { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
