using Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Persistance;
using System.Linq.Expressions;

namespace PensamientoAlternativo.Infrastructure.Repositories;

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

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
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
}