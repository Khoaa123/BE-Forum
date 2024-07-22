namespace Be_Voz_Clone.src.Model.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Forum> Forums { get; set; }
    }
}
