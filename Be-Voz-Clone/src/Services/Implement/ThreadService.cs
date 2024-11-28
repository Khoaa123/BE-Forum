using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ThreadService : IThreadService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ThreadService(IMapper mapper, AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
    }

    public async Task<ThreadObjectResponse> CreateAsync(ThreadRequest request)
    {
        ThreadObjectResponse response = new();
        var thread = _mapper.Map<VozThread>(request);
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();
        thread.CreatedAt = localTime;
        await _context.Threads.AddAsync(thread);
        await _context.SaveChangesAsync();
        var createdThread = await _context.Threads
            .Include(t => t.Forum)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == thread.Id);
        response.StatusCode = ResponseCode.CREATED;
        //response.Message = "Thread created!";
        response.Data = _mapper.Map<ThreadResponse>(createdThread);
        return response;
    }

    public async Task<ThreadObjectResponse> DeleteAsync(int id)
    {
        ThreadObjectResponse response = new();
        var thread = await _context.Threads.FirstOrDefaultAsync(x => x.Id == id);
        if (thread == null) throw new NotFoundException("Thread not found");
        _context.Threads.Remove(thread);
        await _context.SaveChangesAsync();
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Thread deleted!";
        response.Data = _mapper.Map<ThreadResponse>(thread);
        return response;
    }

    public async Task<ThreadObjectResponse> GetAsync(int id, int pageNumber, int pageSize)
    {
        ThreadObjectResponse response = new();
        var thread = await _context.Threads
            .Include(t => t.Comments)
            .ThenInclude(c => c.User)
            .Include(t => t.Comments)
            .ThenInclude(c => c.Reactions)
            .Include(t => t.User)
            .Include(t => t.Forum)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (thread == null) throw new NotFoundException("Thread not found!");
        var skipResults = (pageNumber - 1) * pageSize;
        var totalComments = thread.Comments.Count;
        var totalPages = (int)Math.Ceiling((double)totalComments / pageSize);
        var commentPerPage = thread.Comments
            .Skip(skipResults)
            .Take(pageSize)
            .ToList();
        var threadResponse = _mapper.Map<ThreadResponse>(thread);
        threadResponse.Comments = _mapper.Map<List<CommentResponse>>(commentPerPage);
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Thread retrieved!";
        response.Data = threadResponse;
        response.TotalPages = totalPages;
        return response;
    }

    public async Task<ThreadListObjectResponse> GetThreadsByForumAsync(int forumId, int pageNumber, int pageSize)
    {
        ThreadListObjectResponse response = new();
        var forum = await _context.Forums
            .Include(f => f.Threads)
            .ThenInclude(t => t.User)
            .Include(f => f.Threads)
            .ThenInclude(t => t.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(f => f.Id == forumId);
        if (forum == null) throw new NotFoundException("Category not found!");
        var skipResults = (pageNumber - 1) * pageSize;
        var totalThreads = forum.Threads.Count;
        var totalPages = (int)Math.Ceiling((double)totalThreads / pageSize);
        var threadsPerPage = forum.Threads
            .Skip(skipResults)
            .Take(pageSize)
            .ToList();
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Threads retrieved!";
        response.Data = _mapper.Map<List<ThreadResponse>>(threadsPerPage);
        response.TotalPages = totalPages;
        return response;
    }

    public async Task<ThreadListObjectResponse> GetThreadsByUseridAsync(string userId)
    {
        ThreadListObjectResponse response = new();
        var threads = await _context.Threads
            .Include(x => x.User)
            .Include(x => x.Forum)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        if (!threads.Any()) throw new NotFoundException("Threads not found!");
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Get thread by userId!";
        response.Data = _mapper.Map<List<ThreadResponse>>(threads);
        return response;
    }

    public async Task RecordThreadView(string userId, int threadId)
    {
        var existingView =
            await _context.ViewedThreads.FirstOrDefaultAsync(vt => vt.UserId == userId && vt.ThreadId == threadId);
        var thread = await _context.Threads.FirstOrDefaultAsync(t => t.Id == threadId);
        if (thread == null) throw new Exception("Thread not found.");
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();
        if (existingView == null)
        {
            var viewedThread = new ViewedThread
            {
                UserId = userId,
                ThreadId = threadId,
                ViewedAt = localTime
            };
            await _context.ViewedThreads.AddAsync(viewedThread);
            thread.ViewCount += 1;
        }
        else
        {
            existingView.ViewedAt = localTime;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            user.LastActivity = localTime;
            await _userManager.UpdateAsync(user);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ThreadObjectResponse> ToggleStickyAsync(int id)
    {
        ThreadObjectResponse response = new();
        var thread = await _context.Threads.FirstOrDefaultAsync(x => x.Id == id);

        if (thread == null)
        {
            throw new NotFoundException("Thread not found");
        }

        thread.IsSticky = !thread.IsSticky;

        _context.Threads.Update(thread);
        await _context.SaveChangesAsync();
        response.StatusCode = ResponseCode.OK;
        //response.Message = "";
        response.Data = _mapper.Map<ThreadResponse>(thread);
        return response;
    }
}