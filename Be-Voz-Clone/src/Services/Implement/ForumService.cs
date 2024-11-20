using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Forum;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ForumService : IForumService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ForumService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ForumObjectResponse> CreateAsync(ForumRequest request)
    {
        ForumObjectResponse response = new();
        var forum = _mapper.Map<Forum>(request);
        await _context.Forums.AddAsync(forum);
        await _context.SaveChangesAsync();
        var createdForum = await _context.Forums
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id == forum.Id);
        response.StatusCode = ResponseCode.CREATED;
        response.Message = "Forum created!";
        response.Data = _mapper.Map<ForumResponse>(createdForum);
        return response;
    }

    public async Task<ForumObjectResponse> DeleteAsync(int id)
    {
        ForumObjectResponse response = new();
        var forum = await _context.Forums.FirstOrDefaultAsync(x => x.Id == id);
        if (forum == null) throw new NotFoundException("Forum not found!");
        _context.Forums.Remove(forum);
        await _context.SaveChangesAsync();
        response.StatusCode = ResponseCode.OK;
        response.Message = "Forum deleted!";
        response.Data = _mapper.Map<ForumResponse>(forum);
        return response;
    }

    public async Task<ForumListObjectResponse> GetForumsByCategoryAsync(int categoryId)
    {
        ForumListObjectResponse response = new();
        var category = await _context.Categories
            .Include(c => c.Forums)
            .ThenInclude(f => f.Threads)
            .ThenInclude(t => t.User)
            .ThenInclude(f => f.Threads)
            .ThenInclude(t => t.Comments)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
        if (category == null)
        {
            throw new NotFoundException("Category not found!");
        }
        var forums = category.Forums.ToList();
        response.StatusCode = ResponseCode.OK;
        response.Message = "Forums retrieved!";
        response.Data = _mapper.Map<List<ForumResponse>>(forums);
        return response;
    }
}