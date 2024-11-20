using Be_Voz_Clone.src.Services.DTO.Search;

namespace Be_Voz_Clone.src.Services
{
    public interface ISearchService
    {
        Task<SearchListObjectResponse> SearchAsync(string? keyword, string? forum, string? tag, int pageNumber, int pageSize);
    }
}
