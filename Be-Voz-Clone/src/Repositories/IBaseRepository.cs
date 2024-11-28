using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> FindAllAsync();

        Task<List<T>> FindByCondition(Expression<Func<T, bool>>? filter = null);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null);

        Task<T> AddAsync(T entity);

        Task<T> Update(T entity);

        Task<T> Remove(T entity);
    }
}