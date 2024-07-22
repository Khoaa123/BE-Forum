using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<List<T>> FindAllAsync();
        public Task<List<T>> FindByCondition(Expression<Func<T, bool>>? filter = null);
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null);
        public Task<T> AddAsync(T entity);
        public Task<T> Update(T entity);
        public Task<T> Remove(T entity);
    }
}
