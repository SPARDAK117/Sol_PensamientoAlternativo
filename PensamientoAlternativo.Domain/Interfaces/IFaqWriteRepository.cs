using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IFaqWriteRepository
    {
        Task<int> CreateAsync(Faq faq, CancellationToken ct);
        Task<Faq?> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(Faq faq, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
