using Be_Voz_Clone.src.Model.Entities;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories
{
    public interface IThreadRepository : IBaseRepository<VozThread>
    {
        Task<List<VozThread>> QueryThreadsAsync(Expression<Func<VozThread, bool>>? filter = null, Func<IQueryable<VozThread>, IQueryable<VozThread>>? includeProperties = null, Func<IQueryable<VozThread>, IOrderedQueryable<VozThread>>? orderBy = null);
    }
}