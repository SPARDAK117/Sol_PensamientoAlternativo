using PensamientoAlternativo.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IBlogCategoryWriteRepository
    {
        Task<int> CreateAsync(BlogCategory category, CancellationToken ct);
        Task<BlogCategory?> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(BlogCategory category, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
        Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);
        Task<bool> HasArticlesAsync(int id, CancellationToken ct);
    }
}
