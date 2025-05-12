using Be_Voz_Clone.src.Services.Common;

namespace Be_Voz_Clone.src.Services.DTO.Category
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ForumCount { get; set; }
        public List<string> Forums { get; set; }

    }

    public class CategoryObjectResponse : ObjectResponse<CategoryResponse> { }

    public class CategoryListObjectResponse : ObjectResponse<List<CategoryResponse>> { }
}
