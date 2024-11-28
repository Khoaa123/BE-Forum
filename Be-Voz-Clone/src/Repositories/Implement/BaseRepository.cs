using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Thêm mới một entity vào cơ sở dữ liệu
        public async Task<T> AddAsync(T entity)
        {
            var addedEntity = await _dbSet.AddAsync(entity);
            return addedEntity.Entity;
        }

        // Lấy tất cả các entity
        public async Task<List<T>> FindAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Lọc các entity theo điều kiện filter
        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        // Lấy phần tử đầu tiên hoặc mặc định nếu không tìm thấy
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
        {
            var query = _dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        /// Lấy phần tử đầu tiên hoặc mặc định với include
        public async Task<T> FirstOrDefaultWithIncludeAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        // Xóa entity
        public async Task<T> Remove(T entity)
        {
            var removedEntity = _dbSet.Remove(entity).Entity;
            return removedEntity;
        }

        // Cập nhật entity
        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}