using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class ViewedThreadRepository : BaseRepository<ViewedThread>, IViewedThreadRepository
    {
        private readonly AppDbContext _context;

        public ViewedThreadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ViewedThread>> QueryThreadsAsync(Expression<Func<ViewedThread, bool>>? filter = null, Func<IQueryable<ViewedThread>, IQueryable<ViewedThread>>? includeProperties = null, Func<IQueryable<ViewedThread>, IOrderedQueryable<ViewedThread>>? orderBy = null)
        {
            IQueryable<ViewedThread> query = _context.ViewedThreads;

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