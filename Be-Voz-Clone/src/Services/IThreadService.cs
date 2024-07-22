using Be_Voz_Clone.src.Services.DTO.Thread;

namespace Be_Voz_Clone.src.Services
{
    public interface IThreadService
    {
        Task<ThreadObjectResponse> CreateAsync(ThreadRequest request);
        Task<ThreadListObjectResponse> GetThreadsByForumAsync(int forumId, int pageNumber, int pageSize);
        Task<ThreadObjectResponse> DeleteAsync(int id);
    }
}
