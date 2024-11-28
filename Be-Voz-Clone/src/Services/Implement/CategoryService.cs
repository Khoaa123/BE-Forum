using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.Category;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;

namespace Be_Voz_Clone.src.Services.Implement;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryObjectResponse> CreateAsync(CategoryRequest request)
    {
        CategoryObjectResponse response = new();
        var category = _mapper.Map<Category>(request);
        await _categoryRepository.AddAsync(category);
        response.StatusCode = ResponseCode.CREATED;
        response.Data = _mapper.Map<CategoryResponse>(category);
        return response;
    }

    public async Task<CategoryListObjectResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        CategoryListObjectResponse response = new();
        var skipResults = (pageNumber - 1) * pageSize;
        var categories = await _categoryRepository.FindByCondition(null);
        var categoriesPerPage = categories.Skip(skipResults).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling((double)categories.Count / pageSize);

        response.StatusCode = ResponseCode.OK;
        response.Data = _mapper.Map<List<CategoryResponse>>(categoriesPerPage);
        response.TotalPages = totalPages;
        return response;
    }

    public async Task<CategoryObjectResponse> DeleteAsync(int id)
    {
        CategoryObjectResponse response = new();
        var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            throw new NotFoundException("Category not found!");
        }

        await _categoryRepository.Remove(category);
        response.StatusCode = ResponseCode.OK;
        response.Data = _mapper.Map<CategoryResponse>(category);
        return response;
    }
}