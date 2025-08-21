using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance.Repositories
{

    public sealed class ArticleWriteRepository : IArticleWriteRepository
    {
        private readonly AppDbContext _db;
        public ArticleWriteRepository(AppDbContext db) => _db = db;

        public Task<Article?> GetByIdAsync(int id, CancellationToken ct) =>
                _db.Articles.FirstOrDefaultAsync(a => a.Id == id, ct); // tracking por defecto

        public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct) =>
            _db.BlogCategories.AnyAsync(c => c.Id == categoryId, ct);

        public async Task<int> CreateAsync(Article article, CancellationToken ct)
        {
            _db.Articles.Add(article);
            await _db.SaveChangesAsync(ct);
            return article.Id;
        }

        public Task UpdateAsync(Article article, CancellationToken ct) =>
            _db.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _db.Articles
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }
    }
}

