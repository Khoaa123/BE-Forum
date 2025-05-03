using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.SavedThread;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Be_Voz_Clone.src.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class SavedThreadService : ISavedThreadService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SavedThreadService(IMapper mapper, AppDbContext context, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<SavedThreadListObjectResponse> GetSavedThreadsAsync(string userId, int pageNumber, int pageSize)
        {
            SavedThreadListObjectResponse response = new();

            var savedThreadRepository = _unitOfWork.GetRepository<ISavedThreadRepository>();

            var savedThreads = await savedThreadRepository.QueryThreadsAsync(
                st => st.UserId == userId && st.Thread.User != null,
                query => query
                    .Include(st => st.User)
                    .Include(st => st.Thread)
                        .ThenInclude(t => t.Forum)
                    .Include(st => st.Thread)
                        .ThenInclude(t => t.User),
                query => query.OrderByDescending(st => st.SavedAt)
            );

            if (!savedThreads.Any())
            {
                response.StatusCode = ResponseCode.OK;
                response.AddMessage("No saved threads found.");
                response.Data = new List<SavedThreadResponse>();
                response.TotalPages = 0;
                return response;
            }

            var totalThreads = savedThreads.Count;
            var totalPages = (int)Math.Ceiling((double)totalThreads / pageSize);
            var skipResults = (pageNumber - 1) * pageSize;
            var limitedThreads = savedThreads
                .Skip(skipResults)
                .Take(pageSize)
                .ToList();

            response.StatusCode = ResponseCode.OK;
            response.AddMessage("Saved threads retrieved successfully!");
            response.Data = _mapper.Map<List<SavedThreadResponse>>(limitedThreads);
            response.TotalPages = totalPages;
            return response;
        }

        public async Task RemoveSavedThreadAsync(string userId, int threadId)
        {
            var savedThreadRepository = _unitOfWork.GetRepository<ISavedThreadRepository>();

            var savedThread = await savedThreadRepository.FirstOrDefaultAsync(
                st => st.UserId == userId && st.ThreadId == threadId
            );

            if (savedThread == null)
            {
                throw new NotFoundException("Saved thread not found.");
            }

            await savedThreadRepository.Remove(savedThread);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SaveThreadAsync(string userId, int threadId)
        {
            var threadRepository = _unitOfWork.GetRepository<IThreadRepository>();
            var savedThreadRepository = _unitOfWork.GetRepository<ISavedThreadRepository>();

            var thread = await threadRepository.FirstOrDefaultAsync(t => t.Id == threadId);
            if (thread == null)
            {
                throw new NotFoundException($"Thread with ID {threadId} not found.");
            }

            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = await userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            var existingSavedThread = await savedThreadRepository.FirstOrDefaultAsync(
                st => st.UserId == userId && st.ThreadId == threadId
            );

            if (existingSavedThread != null)
            {
                throw new BadRequestException("Thread is already saved.");
            }

            var savedThread = new SavedThread
            {
                UserId = userId,
                ThreadId = threadId,
                SavedAt = DateTime.Now,
            };

            await savedThreadRepository.AddAsync(savedThread);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsThreadSavedAsync(string userId, int threadId)
        {
            var savedThreadRepository = _unitOfWork.GetRepository<ISavedThreadRepository>();
            return await savedThreadRepository.AnyAsync(st => st.UserId == userId && st.ThreadId == threadId);
        }
    }
}
