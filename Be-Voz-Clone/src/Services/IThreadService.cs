using Be_Voz_Clone.src.Services.DTO.Thread;

namespace Be_Voz_Clone.src.Services
{
    public interface IThreadService
    {
        Task<ThreadObjectResponse> CreateAsync(ThreadRequest request);

        Task<ThreadListObjectResponse> GetThreadsByForumAsync(int forumId, int pageNumber, int pageSize);

        Task<ThreadObjectResponse> DeleteAsync(int id);

        Task<ThreadObjectResponse> GetAsync(int id, int pageNumber, int pageSize);

        Task RecordThreadView(string userId, int threadId);

        Task<ThreadListObjectResponse> GetThreadsByUseridAsync(string userId);

        Task<ThreadObjectResponse> ToggleStickyAsync(int id);
        Task<ThreadListObjectResponse> GetLatestThreadsAsync();
        Task<ThreadListObjectResponse> GetTrendingThreadsAsync();
    }
}