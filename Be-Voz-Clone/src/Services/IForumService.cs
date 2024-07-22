using Be_Voz_Clone.src.Services.DTO.Forum;

namespace Be_Voz_Clone.src.Services
{
    public interface IForumService
    {
        Task<ForumObjectResponse> CreateAsync(ForumRequest request);
        Task<ForumListObjectResponse> GetForumsByCategoryAsync(int categoryId);
        Task<ForumObjectResponse> DeleteAsync(int id);
    }
}
