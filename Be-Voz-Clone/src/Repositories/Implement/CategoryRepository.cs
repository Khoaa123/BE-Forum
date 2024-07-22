using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
