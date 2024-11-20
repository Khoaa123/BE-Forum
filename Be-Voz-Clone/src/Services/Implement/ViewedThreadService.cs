using AutoMapper;
using Be_Voz_Clone.src.Services.DTO.ViewedThread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ViewedThreadService : IViewedThreadService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ViewedThreadService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ViewedThreadListObjectResponse> ViewedThreadListAsync(string userId, int limit)
    {
        ViewedThreadListObjectResponse response = new();
        var viewedThreads = await _context.ViewedThreads
            .Include(x => x.User)
            .Include(x => x.Thread)
            .ThenInclude(t => t.Forum)
            .Where(vt => vt.UserId == userId)
            .OrderByDescending(vt => vt.ViewedAt)
            .Take(limit)
            .ToListAsync();
        if (!viewedThreads.Any()) throw new NotFoundException("No viewed threads found.");
        response.StatusCode = ResponseCode.OK;
        response.Message = "Viewed threads retrieved successfully!";
        response.Data = _mapper.Map<List<ViewedThreadResponse>>(viewedThreads);
        return response;
    }
}