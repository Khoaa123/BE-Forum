using Be_Voz_Clone.src.Model.Entities;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories
{
    public interface ISavedThreadRepository : IBaseRepository<SavedThread>
    {
        Task<List<SavedThread>> QueryThreadsAsync(Expression<Func<SavedThread, bool>>? filter = null, Func<IQueryable<SavedThread>, IQueryable<SavedThread>>? includeProperties = null, Func<IQueryable<SavedThread>, IOrderedQueryable<SavedThread>>? orderBy = null);
    }
}
