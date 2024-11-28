using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class ForumRepository : BaseRepository<Forum>, IForumRepository
    {
        private readonly AppDbContext _context;

        public ForumRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}