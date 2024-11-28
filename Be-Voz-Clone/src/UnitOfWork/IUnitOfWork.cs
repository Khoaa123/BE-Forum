namespace Be_Voz_Clone.src.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;

        Task<int> SaveChangesAsync();
    }
}