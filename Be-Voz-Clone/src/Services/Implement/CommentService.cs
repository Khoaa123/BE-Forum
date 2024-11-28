using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Reaction;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;

namespace Be_Voz_Clone.src.Services.Implement;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReactionObjectResponse> AddReactionAsync(ReactionRequest request)
    {
        ReactionObjectResponse response = new ReactionObjectResponse();

        var commentRepository = _unitOfWork.GetRepository<ICommentRepository>();
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        var reactionRepository = _unitOfWork.GetRepository<IReactionRepository>();

        var comment = await commentRepository.FirstOrDefaultAsync(c => c.Id == request.CommentId);
        if (comment == null) throw new NotFoundException("Comment not found!");

        var user = await userRepository.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null) throw new NotFoundException("User not found!");

        var existingReaction = await reactionRepository.FirstOrDefaultAsync(r =>
            r.CommentId == request.CommentId && r.UserId == request.UserId);

        if (existingReaction != null)
        {
            if (existingReaction.Type != request.Type)
            {
                UpdateReactionScore(comment, existingReaction.Type, request.Type);
                existingReaction.Type = request.Type;
                existingReaction.CreatedAt = DateTime.UtcNow;
                await reactionRepository.Update(existingReaction);
                response.StatusCode = ResponseCode.OK;
                response.AddMessage("Reaction updated!");
            }
        }
        else
        {
            var reaction = _mapper.Map<Reaction>(request);
            reaction.CreatedAt = DateTime.UtcNow;
            UpdateReactionScore(comment, null, request.Type);
            await reactionRepository.AddAsync(reaction);
            response.StatusCode = ResponseCode.CREATED;
            response.AddMessage("Reaction added!");
        }

        await _unitOfWork.SaveChangesAsync();
        response.Data = _mapper.Map<ReactionResponse>(existingReaction ?? _mapper.Map<Reaction>(request));
        return response;
    }

    public async Task<CommentObjectResponse> CreateAsync(CommentRequest request)
    {
        CommentObjectResponse response = new CommentObjectResponse();

        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        var commentRepository = _unitOfWork.GetRepository<ICommentRepository>();

        var user = await userRepository.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null) throw new NotFoundException("User not found!");

        var thread = await commentRepository.FirstOrDefaultAsync(t => t.Id == request.ThreadId);
        if (thread == null) throw new NotFoundException("Thread not found!");

        var comment = _mapper.Map<Comment>(request);
        comment.User = user;
        comment.CreatedAt = DateTime.UtcNow;

        await commentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.CREATED;
        response.AddMessage("Comment created!");
        response.Data = _mapper.Map<CommentResponse>(comment);
        return response;
    }

    public async Task<CommentListObjectResponse> GetAllCommentAsync(string userId)
    {
        CommentListObjectResponse response = new CommentListObjectResponse();

        var commentRepository = _unitOfWork.GetRepository<ICommentRepository>();

        var comments = await commentRepository.GetCommentsByUserIdAsync(userId);

        if (comments == null || !comments.Any())
        {
            throw new NotFoundException("Comments not found!");
        }

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Get all comment");
        response.Data = _mapper.Map<List<CommentResponse>>(comments);
        return response;
    }

    public async Task<CommentObjectResponse> GetCommentId(int id)
    {
        CommentObjectResponse response = new CommentObjectResponse();

        var commentRepository = _unitOfWork.GetRepository<ICommentRepository>();

        var comment = await commentRepository.GetCommentWithUserAndThreadAsync(id);
        if (comment == null) throw new NotFoundException("Comment not found!");

        response.StatusCode = ResponseCode.OK;
        response.AddMessage("Get comment");
        response.Data = _mapper.Map<CommentResponse>(comment);
        return response;
    }

    public async Task<CommentObjectResponse> ReplyAsync(CommentRequest request)
    {
        CommentObjectResponse response = new CommentObjectResponse();

        var commentRepository = _unitOfWork.GetRepository<ICommentRepository>();

        var parentComment = await commentRepository.FirstOrDefaultAsync(c => c.Id == request.ParentCommentId);
        if (parentComment == null) throw new NotFoundException("Parent comment not found!");

        var thread = await commentRepository.FirstOrDefaultAsync(t => t.Id == request.ThreadId);
        if (thread == null) throw new NotFoundException("Thread not found!");

        var replyComment = _mapper.Map<Comment>(request);
        replyComment.CreatedAt = DateTime.UtcNow;
        replyComment.ParentCommentId = request.ParentCommentId;

        await commentRepository.AddAsync(replyComment);
        await _unitOfWork.SaveChangesAsync();

        response.StatusCode = ResponseCode.CREATED;
        response.AddMessage("Reply comment created!");
        response.Data = _mapper.Map<CommentResponse>(replyComment);
        return response;
    }

    private void UpdateReactionScore(Comment comment, ReactionType? oldType, ReactionType newType)
    {
        if (oldType == ReactionType.Like)
            comment.User.ReactionScore -= 1;
        else if (oldType == ReactionType.Dislike)
            comment.User.ReactionScore += 1;

        if (newType == ReactionType.Like)
            comment.User.ReactionScore += 1;
        else if (newType == ReactionType.Dislike)
            comment.User.ReactionScore -= 1;
    }
}