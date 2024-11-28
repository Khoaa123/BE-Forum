using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class ThreadRepository : BaseRepository<VozThread>, IThreadRepository
    {
        private readonly AppDbContext _context;

        public ThreadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<VozThread>> QueryThreadsAsync(Expression<Func<VozThread, bool>>? filter = null, Func<IQueryable<VozThread>, IQueryable<VozThread>>? includeProperties = null, Func<IQueryable<VozThread>, IOrderedQueryable<VozThread>>? orderBy = null)
        {
            IQueryable<VozThread> query = _context.Threads;

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