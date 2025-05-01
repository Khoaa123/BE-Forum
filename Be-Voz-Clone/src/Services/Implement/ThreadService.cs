using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ThreadService : IThreadService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ThreadService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ThreadObjectResponse> CreateAsync(ThreadRequest request)
    {
        ThreadObjectResponse response = new ThreadObjectResponse();
        var thread = _mapper.Map<VozThread>(request);
        thread.CreatedAt = DateTime.UtcNow.ToLocalTime();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var createdThread = await threadRepository.AddAsync(thread);
        await _unitOfWork.SaveChangesAsync();

        var threadWithDetails = await threadRepository.FirstOrDefaultWithIncludeAsync(
            t => t.Id == createdThread.Id,
            query => query
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.Reactions)
                .Include(t => t.User)
                .Include(t => t.Forum)
        );

        response.StatusCode = ResponseCode.CREATED;
        response.AddMessage("Thread created!");
        response.Data = _mapper.Map<ThreadResponse>(threadWithDetails);
        return response;
    }

    public async Task<ThreadObjectResponse> DeleteAsync(int id)
    {
        ThreadObjectResponse response = new ThreadObjectResponse();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();
        var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == id);
        if (thread == null)
        {
            throw new NotFoundException("Thread not found");
        }

        await threadRepository.Remove(thread);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Thread deleted!");
        response.Data = _mapper.Map<ThreadResponse>(thread);
        return response;
    }

    public async Task<ThreadObjectResponse> GetAsync(int id, int pageNumber, int pageSize)
    {
        ThreadObjectResponse response = new ThreadObjectResponse();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var thread = await threadRepository.FirstOrDefaultWithIncludeAsync(
            t => t.Id == id,
            query => query
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.Reactions)
                .Include(t => t.User)
                .Include(t => t.Forum)
        );

        if (thread == null)
        {
            throw new NotFoundException("Thread not found!");
        }

        var skipResults = (pageNumber - 1) * pageSize;
        var totalComments = thread.Comments.Count;
        var totalPages = (int)Math.Ceiling((double)totalComments / pageSize);

        var comments = thread.Comments
            .Skip(skipResults)
            .Take(pageSize)
            .ToList();

        var threadResponse = _mapper.Map<ThreadResponse>(thread);
        threadResponse.Comments = _mapper.Map<List<CommentResponse>>(comments);

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Thread retrieved");
        response.Data = threadResponse;
        response.TotalPages = totalPages;
        return response;
    }

    public async Task<ThreadListObjectResponse> GetLatestThreadsAsync()
    {
        ThreadListObjectResponse response = new ThreadListObjectResponse();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var threads = await threadRepository.QueryThreadsAsync(
            filter: t => !t.IsHidden && t.DeletedAt == null,
            includeProperties: query => query
                .Include(t => t.User),
            orderBy: query => query.OrderByDescending(t => t.CreatedAt)
        );

        var latestThreads = threads.Take(3).ToList();

        if (!latestThreads.Any())
        {
            throw new NotFoundException("No threads found!");
        }

        var threadResponses = _mapper.Map<List<ThreadResponse>>(latestThreads);

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Latest threads retrieved!");
        response.Data = threadResponses;

        return response;
    }

    public async Task<ThreadListObjectResponse> GetThreadsByForumAsync(int forumId, int pageNumber, int pageSize)
    {
        ThreadListObjectResponse response = new ThreadListObjectResponse();

        var forumRepository = _unitOfWork.GetRepository<IForumRepository>();

        var forum = await forumRepository.FirstOrDefaultWithIncludeAsync(
            f => f.Id == forumId,
            query => query
                .Include(f => f.Threads)
                    .ThenInclude(t => t.User)
                .Include(f => f.Threads)
                    .ThenInclude(t => t.Comments)
                    .ThenInclude(c => c.User)
        );

        if (forum == null)
        {
            throw new NotFoundException("Category not found!");
        }

        var skipResults = (pageNumber - 1) * pageSize;
        var totalThreads = forum.Threads.Count;
        var totalPages = (int)Math.Ceiling((double)totalThreads / pageSize);
        var threadsPerPage = forum.Threads
            .Skip(skipResults)
            .Take(pageSize)
            .ToList();

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Threads retrieved!");
        response.Data = _mapper.Map<List<ThreadResponse>>(threadsPerPage);
        response.TotalPages = totalPages;

        return response;
    }


    public async Task<ThreadListObjectResponse> GetThreadsByUseridAsync(string userId)
    {
        ThreadListObjectResponse response = new ThreadListObjectResponse();
        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var threads = await threadRepository.QueryThreadsAsync(
            t => t.UserId == userId,
            query => query
            .Include(t => t.User)
            .Include(t => t.Forum),
            query => query.OrderByDescending(t => t.CreatedAt)
        );

        if (!threads.Any())
        {
            throw new NotFoundException("Threads not found!");
        }

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Get thread by userId");
        response.Data = _mapper.Map<List<ThreadResponse>>(threads);
        return response;
    }

    public async Task<ThreadListObjectResponse> GetTrendingThreadsAsync()
    {
        ThreadListObjectResponse response = new ThreadListObjectResponse();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var threads = await threadRepository.QueryThreadsAsync(
            filter: t => !t.IsHidden && t.DeletedAt == null,
            query => query
            .Include(t => t.User)
            .Include(t => t.Forum),
            query => query.OrderByDescending(t => t.ViewCount)
        );

        var trendingThreads = threads.Take(3).ToList();

        if (!trendingThreads.Any())
        {
            throw new NotFoundException("No trending threads found!");
        }

        var threadResponses = _mapper.Map<List<ThreadResponse>>(trendingThreads);

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Trending threads retrieved!");
        response.Data = threadResponses;

        return response;
    }

    public async Task RecordThreadView(string userId, int threadId)
    {
        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();
        var viewedThreadRepository = _unitOfWork.GetRepository<IViewedThreadRepository>();

        var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId);
        if (thread == null)
        {
            throw new Exception("Thread not found.");
        }

        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();

        var existingView = await viewedThreadRepository.FirstOrDefaultAsync(vt => vt.UserId == userId && vt.ThreadId == threadId);

        if (existingView == null)
        {
            var viewedThread = new ViewedThread
            {
                UserId = userId,
                ThreadId = threadId,
                ViewedAt = localTime
            };

            await viewedThreadRepository.AddAsync(viewedThread);
            thread.ViewCount += 1;
            await threadRepository.Update(thread);
        }
        else
        {
            existingView.ViewedAt = localTime;
            await viewedThreadRepository.Update(existingView);
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.LastActivity = localTime;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ThreadObjectResponse> ToggleStickyAsync(int id)
    {
        ThreadObjectResponse response = new ThreadObjectResponse();

        var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();

        var thread = await threadRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (thread == null)
        {
            throw new NotFoundException("Thread not found");
        }

        thread.IsSticky = !thread.IsSticky;

        await threadRepository.Update(thread);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.OK;
        response.Data = _mapper.Map<ThreadResponse>(thread);
        return response;
    }
}