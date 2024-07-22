using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Category;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CategoryService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<CategoryObjectResponse> CreateAsync(CategoryRequest request)
        {
            CategoryObjectResponse response = new();

            var category = _mapper.Map<Category>(request);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            response.StatusCode = ResponseCode.CREATED;
            response.Message = "Category created!";
            response.Data = _mapper.Map<CategoryResponse>(category);

            return response;
        }

        public async Task<CategoryObjectResponse> DeleteAsync(int id)
        {
            CategoryObjectResponse response = new();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new NotFoundException("Category not found!");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            response.StatusCode = ResponseCode.OK;
            response.Message = "Category deleted!";
            response.Data = _mapper.Map<CategoryResponse>(category);

            return response;
        }

        public async Task<CategoryListObjectResponse> GetAllAsync(int pageNumber, int pageSize)
        {
            CategoryListObjectResponse response = new();

            var skipResults = (pageNumber - 1) * pageSize;

            var totalProducts = await _context.Categories.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var categories = await _context.Categories.ToListAsync();

            var categoriesPerPage = categories.Skip(skipResults).Take(pageSize).ToList();

            response.StatusCode = ResponseCode.OK;
            response.Message = "Categories retrieved!";
            response.Data = _mapper.Map<List<CategoryResponse>>(categoriesPerPage);
            response.TotalPages = totalPages;

            return response;
        }
    }
}
