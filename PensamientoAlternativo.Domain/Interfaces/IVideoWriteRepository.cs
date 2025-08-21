using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PensamientoAlternativo.Domain.Entities.Sections;

namespace PensamientoAlternativo.Domain.Interfaces
{
    // Application/Interfaces/IVideoWriteRepository.cs

    public interface IVideoWriteRepository
    {
        Task<int> CreateAsync(Video video, CancellationToken ct);
        Task<Video?> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(Video video, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }

}
