using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class ThreadService : IThreadService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ThreadService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ThreadObjectResponse> CreateAsync(ThreadRequest request)
        {
            ThreadObjectResponse response = new();

            var thread = _mapper.Map<VozThread>(request);

            await _context.Threads.AddAsync(thread);
            await _context.SaveChangesAsync();

            var createdThread = await _context.Threads
                .Include(t => t.Forum)
                .FirstOrDefaultAsync(t => t.Id == thread.Id);

            response.StatusCode = ResponseCode.CREATED;
            response.Message = "Thread created!";
            response.Data = _mapper.Map<ThreadResponse>(createdThread);

            return response;
        }

        public async Task<ThreadObjectResponse> DeleteAsync(int id)
        {
            ThreadObjectResponse response = new();

            var thread = await _context.Threads.FirstOrDefaultAsync(x => x.Id == id);

            if (thread == null)
            {
                throw new NotFoundException("Thread not found");
            }

            _context.Threads.Remove(thread);
            await _context.SaveChangesAsync();

            response.StatusCode = ResponseCode.OK;
            response.Message = "Thread deleted!";
            response.Data = _mapper.Map<ThreadResponse>(thread);

            return response;
        }

        public async Task<ThreadListObjectResponse> GetThreadsByForumAsync(int forumId, int pageNumber, int pageSize)
        {
            ThreadListObjectResponse response = new();

            var forum = await _context.Forums
                .Include(f => f.Threads)
                .FirstOrDefaultAsync(f => f.Id == forumId);

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
            response.Message = "Threads retrieved!";
            response.Data = _mapper.Map<List<ThreadResponse>>(threadsPerPage);
            response.TotalPages = totalPages;

            return response;
        }
    }
}
