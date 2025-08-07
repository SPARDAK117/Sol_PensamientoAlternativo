using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    Task<List<Service>> GetAllServicesAsync(CancellationToken cancellationToken);
}
