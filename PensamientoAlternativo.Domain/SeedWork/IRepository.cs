using System.Linq.Expressions;

namespace Domain.Seedwork
{
    public interface IRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
        Task<List<T>> GetPageAsync(int page, int pageSize, CancellationToken ct = default);
        Task<int> CountAsync(CancellationToken ct = default);
    }

}
