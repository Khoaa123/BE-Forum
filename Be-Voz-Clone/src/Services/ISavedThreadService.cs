using Be_Voz_Clone.src.Services.DTO.SavedThread;

namespace Be_Voz_Clone.src.Services
{
    public interface ISavedThreadService
    {
        Task SaveThreadAsync(string userId, int threadId);
        Task<SavedThreadListObjectResponse> GetSavedThreadsAsync(string userId, int pageNumber, int pageSize);
        Task RemoveSavedThreadAsync(string userId, int threadId);
        Task<bool> IsThreadSavedAsync(string userId, int threadId);
    }
}
