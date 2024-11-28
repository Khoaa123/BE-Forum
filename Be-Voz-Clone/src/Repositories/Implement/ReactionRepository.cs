using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
    {
        private readonly AppDbContext _context;

        public ReactionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}