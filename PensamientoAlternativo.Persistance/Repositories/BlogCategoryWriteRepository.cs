using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities.Blog;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance.Repositories
{
    public sealed class BlogCategoryWriteRepository : IBlogCategoryWriteRepository
    {
        private readonly AppDbContext _db;
        public BlogCategoryWriteRepository(AppDbContext db) => _db = db;

        public async Task<int> CreateAsync(BlogCategory category, CancellationToken ct)
        {
            _db.BlogCategories.Add(category);
            await _db.SaveChangesAsync(ct);
            return category.Id;
        }

        public Task<BlogCategory?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.BlogCategories.Include(c => c.Articles)
                              .FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task UpdateAsync(BlogCategory category, CancellationToken ct) =>
            _db.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _db.BlogCategories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(ct);   // EF Core 7+
            return affected > 0;
        }

        public Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var q = _db.BlogCategories.AsQueryable();
            if (excludeId.HasValue) q = q.Where(c => c.Id != excludeId.Value);
            // Postgres: ILIKE (case-insensitive). Si no usas Npgsql, usa ToLower().
            return q.AnyAsync(c => EF.Functions.ILike(c.Name, name), ct);
        }

        public Task<bool> HasArticlesAsync(int id, CancellationToken ct) =>
            _db.Articles.AnyAsync(a => a.CategoryId == id, ct);
    }
}
