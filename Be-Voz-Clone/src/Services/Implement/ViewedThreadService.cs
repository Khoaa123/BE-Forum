using AutoMapper;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.ViewedThread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class ViewedThreadService : IViewedThreadService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ViewedThreadService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ViewedThreadListObjectResponse> ViewedThreadListAsync(string userId, int limit)
    {
        ViewedThreadListObjectResponse response = new();

        var viewedThreadRepository = _unitOfWork.GetRepository<IViewedThreadRepository>();

        var viewedThreads = await viewedThreadRepository.QueryThreadsAsync(
            vt => vt.UserId == userId,
            query => query
                .Include(x => x.User)
                .Include(x => x.Thread)
                    .ThenInclude(t => t.Forum)
                .OrderByDescending(vt => vt.ViewedAt)
                .Take(limit)
        );

        if (!viewedThreads.Any()) throw new NotFoundException("No viewed threads found.");

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Viewed threads retrieved successfully!");
        response.Data = _mapper.Map<List<ViewedThreadResponse>>(viewedThreads);
        return response;
    }
}