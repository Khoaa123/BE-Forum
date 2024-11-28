using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _context.Comments
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentWithUserAndThreadAsync(int id)
        {
            return await _context.Comments
                .Include(x => x.User)
                .Include(x => x.Thread)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}