using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.SavedThread
{
    public class SavedThreadResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Tag { get; set; }
        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        public string ThreadContent { get; set; }
        public string ForumName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime SavedAt { get; set; }
    }

    public class SavedThreadListObjectResponse : ObjectResponse<List<SavedThreadResponse>> { }

}
