using Be_Voz_Clone.src.Services.DTO.Category;

namespace Be_Voz_Clone.src.Services
{
    public interface ICategoryService
    {
        Task<CategoryObjectResponse> CreateAsync(CategoryRequest request);

        Task<CategoryListObjectResponse> GetAllAsync(int pageNumber, int pageSize);

        Task<CategoryObjectResponse> DeleteAsync(int id);
    }
}