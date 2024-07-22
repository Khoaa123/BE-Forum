using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.Thread
{
    public class ThreadResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSticky { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public string Tag { get; set; }
    }

    public class ThreadListObjectResponse : ObjectResponse<List<ThreadResponse>> { }
    public class ThreadObjectResponse : ObjectResponse<ThreadResponse> { }
}
