using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.Forum;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ForumService : IForumService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ForumService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ForumObjectResponse> CreateAsync(ForumRequest request)
    {
        ForumObjectResponse response = new();
        var forum = _mapper.Map<Forum>(request);

        var forumRepository = _unitOfWork.GetRepository<IForumRepository>();

        var createdForum = await forumRepository.AddAsync(forum);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.CREATED;
        response.AddMessage("Forum created!");
        response.Data = _mapper.Map<ForumResponse>(createdForum);

        return response;
    }

    public async Task<ForumObjectResponse> DeleteAsync(int id)
    {
        ForumObjectResponse response = new();

        var forumRepository = _unitOfWork.GetRepository<IForumRepository>();

        var forum = await forumRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (forum == null) throw new NotFoundException("Forum not found!");

        var deletedForum = await forumRepository.Remove(forum);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Forum deleted!");
        response.Data = _mapper.Map<ForumResponse>(deletedForum);

        return response;
    }

    public async Task<ForumListObjectResponse> GetAllForumsAsync()
    {
        ForumListObjectResponse response = new ForumListObjectResponse();

        var forumRepository = _unitOfWork.GetRepository<IForumRepository>();

        var forums = await forumRepository.FindByConditionWithIncludeAsync(
            null,
            query => query
                .Include(f => f.Category)
                .Include(f => f.Threads)
                    .ThenInclude(t => t.Comments)
                .Include(f => f.Threads)
                    .ThenInclude(t => t.User)
        );

        if (forums == null || !forums.Any())
        {
            throw new NotFoundException("No forums found");
        }

        response.AddMessage("Forums retrieved successfully!");
        response.Data = _mapper.Map<List<ForumResponse>>(forums);
        return response;
    }

    public async Task<ForumListObjectResponse> GetForumsByCategoryAsync(int categoryId)
    {
        ForumListObjectResponse response = new ForumListObjectResponse();

        var forumRepository = _unitOfWork.GetRepository<IForumRepository>();

        var forums = await forumRepository.FindByConditionWithIncludeAsync(
            f => f.CategoryId == categoryId,
            query => query
                .Include(f => f.Category)
                .Include(f => f.Threads)
                    .ThenInclude(t => t.Comments)
                .Include(f => f.Threads)
                    .ThenInclude(t => t.User)
        );
        if (forums == null || !forums.Any())
        {
            throw new NotFoundException("No forums found for this category!");
        }

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Forum retrieved!");
        response.Data = _mapper.Map<List<ForumResponse>>(forums);

        return response;
    }
}