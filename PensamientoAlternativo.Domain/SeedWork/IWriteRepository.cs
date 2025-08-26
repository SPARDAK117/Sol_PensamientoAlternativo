using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.SeedWork
{
    public interface IWriteRepository<T> where T : Entity
    {
        Task UpdateAsync(Service service,CancellationToken ct);

        Task<bool> DeleteAsync(int id,CancellationToken ct);
    }
}
