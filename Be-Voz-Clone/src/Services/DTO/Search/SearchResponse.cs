using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.Search
{
    public class SearchResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string ForumName { get; set; }
    }

    public class SearchListObjectResponse : ObjectResponse<List<SearchResponse>> { }
}
