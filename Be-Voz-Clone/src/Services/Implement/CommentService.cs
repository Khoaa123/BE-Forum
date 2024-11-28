using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Reaction;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CommentService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReactionObjectResponse> AddReactionAsync(ReactionRequest request)
    {
        ReactionObjectResponse response = new();
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(x => x.Id == request.CommentId);
        if (comment == null) throw new NotFoundException("Comment not found!");
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new NotFoundException("User not found!");
        var existingReaction =
            await _context.Reactions.FirstOrDefaultAsync(x =>
                x.CommentId == request.CommentId && x.UserId == request.UserId);
        if (existingReaction != null)
        {
            if (existingReaction.Type != request.Type)
            {
                if (comment.User != null)
                {
                    if (existingReaction.Type == ReactionType.Like)
                        comment.User.ReactionScore -= 1;
                    else if (existingReaction.Type == ReactionType.Dislike) comment.User.ReactionScore += 1;
                    if (request.Type == ReactionType.Like)
                        comment.User.ReactionScore += 1;
                    else if (request.Type == ReactionType.Dislike) comment.User.ReactionScore -= 1;
                    existingReaction.Type = request.Type;
                    existingReaction.CreatedAt = localTime;
                    await _context.SaveChangesAsync();
                    response.StatusCode = ResponseCode.OK;
                    //response.Message = "Reaction updated!";
                    response.Data = _mapper.Map<ReactionResponse>(existingReaction);
                }
                else
                {
                    throw new InvalidOperationException("User is not associated with the comment.");
                }
            }
        }
        else
        {
            var reaction = _mapper.Map<Reaction>(request);
            reaction.CreatedAt = localTime;
            if (comment.User != null)
            {
                if (request.Type == ReactionType.Like)
                    comment.User.ReactionScore += 1;
                else if (request.Type == ReactionType.Dislike) comment.User.ReactionScore -= 1;
            }
            else
            {
                throw new InvalidOperationException("User is not associated with the comment.");
            }

            await _context.Reactions.AddAsync(reaction);
            await _context.SaveChangesAsync();
            response.StatusCode = ResponseCode.CREATED;
            //response.Message = "Reaction added!";
            response.Data = _mapper.Map<ReactionResponse>(reaction);
        }

        return response;
    }

    public async Task<CommentObjectResponse> CreateAsync(CommentRequest request)
    {
        CommentObjectResponse response = new();
        var thread = await _context.Threads.FirstOrDefaultAsync(x => x.Id == request.ThreadId);
        if (thread == null) throw new NotFoundException("Thread not found!");
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new NotFoundException("User not found!");
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();
        var comment = _mapper.Map<Comment>(request);
        comment.User = user;
        comment.CreatedAt = localTime;
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        response.StatusCode = ResponseCode.CREATED;
        //response.Message = "Comment created!";
        response.Data = _mapper.Map<CommentResponse>(comment);
        return response;
    }

    public async Task<CommentListObjectResponse> GetAllCommentAsync(string userId)
    {
        CommentListObjectResponse response = new();
        var comments = await _context.Comments
            .Include(x => x.User)
            .Where(x => x.UserId == userId).ToListAsync();
        if (comments == null) throw new NotFoundException("Comments not found!");
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Get all comment";
        response.Data = _mapper.Map<List<CommentResponse>>(comments);
        return response;
    }

    public async Task<CommentObjectResponse> GetCommentId(int id)
    {
        CommentObjectResponse response = new();
        var comment = await _context.Comments
            .Include(x => x.User)
            .Include(x => x.Thread)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (comment == null) throw new NotFoundException("Comment not found!");
        response.StatusCode = ResponseCode.OK;
        //response.Message = "Get comment";
        response.Data = _mapper.Map<CommentResponse>(comment);
        return response;
    }

    public async Task<CommentObjectResponse> ReplyAsync(CommentRequest request)
    {
        CommentObjectResponse response = new();
        var parentComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.ParentCommentId);
        if (parentComment == null) throw new NotFoundException("Parent comment not found!");
        var thread = await _context.Threads.FirstOrDefaultAsync(x => x.Id == request.ThreadId);
        if (thread == null) throw new NotFoundException("Thread not found!");
        var replyComment = _mapper.Map<Comment>(request);
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ToLocalTime();
        replyComment.ParentCommentId = request.ParentCommentId;
        replyComment.CreatedAt = localTime;
        await _context.Comments.AddAsync(replyComment);
        await _context.SaveChangesAsync();
        response.StatusCode = ResponseCode.CREATED;
        //response.Message = "Reply comment created!";
        response.Data = _mapper.Map<CommentResponse>(replyComment);
        return response;
    }
}