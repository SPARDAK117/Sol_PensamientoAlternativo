using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance.Repositories
{
    public sealed class OpinionWriteRepository : IOpinionWriteRepository
    {
        private readonly AppDbContext _db;
        public OpinionWriteRepository(AppDbContext db) => _db = db;

        public async Task<int> CreateAsync(Opinion opinion, CancellationToken ct)
        {
            _db.Opinions.Add(opinion);
            await _db.SaveChangesAsync(ct);
            return opinion.Id;
        }

        public Task<Opinion?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.Opinions.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task UpdateAsync(Opinion opinion, CancellationToken ct) =>
            _db.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            // EF7: eliminación directa
            var affected = await _db.Opinions.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
            return affected > 0;
        }
    }
}
