using Be_Voz_Clone.src.Model.Entities;

namespace Be_Voz_Clone.src.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByUserIdAsync(string userId);

        Task<Comment> GetCommentWithUserAndThreadAsync(int id);
    }
}