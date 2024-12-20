using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.Category;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryObjectResponse> CreateAsync(CategoryRequest request)
    {
        CategoryObjectResponse response = new();
        var category = _mapper.Map<Category>(request);

        var categoryRepository = _unitOfWork.GetRepository<ICategoryRepository>();

        await categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.CREATED;
        response.Data = _mapper.Map<CategoryResponse>(category);
        return response;
    }

    public async Task<CategoryListObjectResponse> GetAllAsync(int pageNumber, int pageSize)
    {
        CategoryListObjectResponse response = new();
        var skipResults = (pageNumber - 1) * pageSize;

        var categoryRepository = _unitOfWork.GetRepository<ICategoryRepository>();

        var categories = await categoryRepository.FindByConditionWithIncludeAsync(
               null,
               query => query.Include(c => c.Forums)
        );
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

        var categoryRepository = _unitOfWork.GetRepository<ICategoryRepository>();

        var category = await categoryRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            throw new NotFoundException("Category not found!");
        }

        await categoryRepository.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.OK;
        response.Data = _mapper.Map<CategoryResponse>(category);
        return response;
    }
}