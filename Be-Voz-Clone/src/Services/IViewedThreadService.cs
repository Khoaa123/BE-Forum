using Be_Voz_Clone.src.Services.DTO.ViewedThread;

namespace Be_Voz_Clone.src.Services
{
    public interface IViewedThreadService
    {
        Task<ViewedThreadListObjectResponse> ViewedThreadListAsync(string userId, int limit);
    }
}
