using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Reaction;

namespace Be_Voz_Clone.src.Services
{
    public interface ICommentService
    {
        Task<CommentObjectResponse> CreateAsync(CommentRequest request);
        Task<CommentObjectResponse> ReplyAsync(CommentRequest request);
        Task<ReactionObjectResponse> AddReactionAsync(ReactionRequest request);
        Task<CommentListObjectResponse> GetAllCommentAsync(string userId);
        Task<CommentObjectResponse> GetCommentId(int id);
    }
}
