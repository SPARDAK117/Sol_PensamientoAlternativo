using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IArticleWriteRepository
    {
        Task<Article?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct);
        Task<int> CreateAsync(Article article, CancellationToken ct);
        Task UpdateAsync(Article article, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
