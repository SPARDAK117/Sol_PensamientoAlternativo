using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IImageWriteRepository
    {
        Task<Image?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> ImageExistsAsync(int imageId, CancellationToken ct);
        Task<int> CreateAsync(Image article, CancellationToken ct);
        Task UpdateAsync(Image article, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
