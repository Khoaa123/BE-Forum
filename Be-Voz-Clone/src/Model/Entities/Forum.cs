namespace Be_Voz_Clone.src.Model.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<VozThread> Threads { get; set; }
    }
}
