using AutoMapper;
using Be_Voz_Clone.src.Services.DTO.Search;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class SearchService : ISearchService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SearchService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<SearchListObjectResponse> SearchAsync(string? keyword, string? forum, string? tag, int pageNumber,
        int pageSize)

    {
        SearchListObjectResponse response = new();
        try
        {
            var threads = _context.Threads.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                threads = threads.Where(x =>
                    EF.Functions.ILike(x.Title, $"%{keyword}%") || EF.Functions.ILike(x.Content, $"%{keyword}%"));
            if (!string.IsNullOrWhiteSpace(forum))
                threads = threads.Include(x => x.Forum).Where(x => x.Forum != null && x.Forum.Name.Contains(forum));
            if (!string.IsNullOrWhiteSpace(tag)) threads = threads.Where(x => x.Tag.Contains(tag));

            var totalThreads = await threads.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalThreads / pageSize);

            var skipResults = (pageNumber - 1) * pageSize;
            var pagedThreadsQuery = threads
                .Skip(skipResults)
                .Take(pageSize);

            var result = await pagedThreadsQuery
                .Include(t => t.Forum)
                .Include(t => t.User)
                .ToListAsync();
            if (result == null || result.Count == 0) throw new NotFoundException("No threads found");
            response.StatusCode = ResponseCode.OK;
            response.Message = "Threads retrieved successfully";
            response.Data = _mapper.Map<List<SearchResponse>>(result);
            response.TotalPages = totalPages;
        }
        catch (Exception ex)
        {
            response.StatusCode = ResponseCode.BADREQUEST;
            response.Message = "Search failed: " + ex.Message;
        }

        return response;
    }
}