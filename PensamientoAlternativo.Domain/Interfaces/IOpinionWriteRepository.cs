using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IOpinionWriteRepository
    {
        Task<int> CreateAsync(Opinion opinion, CancellationToken ct);
        Task<Opinion?> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(Opinion opinion, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
