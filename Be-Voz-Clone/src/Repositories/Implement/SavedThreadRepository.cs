using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class SavedThreadRepository : BaseRepository<SavedThread>, ISavedThreadRepository
    {
        private readonly AppDbContext _context;

        public SavedThreadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SavedThread>> QueryThreadsAsync(Expression<Func<SavedThread, bool>>? filter = null, Func<IQueryable<SavedThread>, IQueryable<SavedThread>>? includeProperties = null, Func<IQueryable<SavedThread>, IOrderedQueryable<SavedThread>>? orderBy = null)
        {
            IQueryable<SavedThread> query = _context.SavedThreads;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }
    }
}
