using Be_Voz_Clone.src.Model.Entities;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories
{
    public interface IViewedThreadRepository : IBaseRepository<ViewedThread>
    {
        Task<List<ViewedThread>> QueryThreadsAsync(Expression<Func<ViewedThread, bool>>? filter = null, Func<IQueryable<ViewedThread>, IQueryable<ViewedThread>>? includeProperties = null, Func<IQueryable<ViewedThread>, IOrderedQueryable<ViewedThread>>? orderBy = null);

    }
}