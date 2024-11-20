using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Services.DTO.Thread;

namespace Be_Voz_Clone.src.Services.DTO.Forum
{
    public class ForumResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int ThreadCount { get; set; }
        public LatestThreadResponse LatestThread { get; set; }
        public int? TotalComments { get; set; }
    }

    public class ForumObjectResponse : ObjectResponse<ForumResponse> { }

    public class ForumListObjectResponse : ObjectResponse<List<ForumResponse>> { }
}
