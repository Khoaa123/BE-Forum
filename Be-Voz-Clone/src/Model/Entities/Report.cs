namespace Be_Voz_Clone.src.Model.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; }
        public int ThreadId { get; set; }
        public VozThread Thread { get; set; }
        public string ReporterUserId { get; set; }
        public ApplicationUser ReporterUser { get; set; }
    }
}
