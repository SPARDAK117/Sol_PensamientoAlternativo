using Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance.Repositories
{
    public  class WriteRepository<T> :IWriteRepository<T> where T : Entity
    {
        private readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Service service, CancellationToken ct)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _context.Services
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }

    }
}
