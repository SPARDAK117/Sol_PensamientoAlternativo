using Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PensamientoAlternativo.Persistance.Repositories;

public abstract class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly AppDbContext _context;

    protected Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return _context.Set<T>()
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public virtual async Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    public virtual Task<List<T>> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return _context.Set<T>()
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public virtual Task<int> CountAsync(CancellationToken ct = default)
    {
        return _context.Set<T>().CountAsync(ct);
    }

}