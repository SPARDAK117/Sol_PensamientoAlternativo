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
    public sealed class FaqWriteRepository : IFaqWriteRepository
    {
        private readonly AppDbContext _db;
        public FaqWriteRepository(AppDbContext db) => _db = db;

        public async Task<int> CreateAsync(Faq faq, CancellationToken ct)
        {
            _db.Faqs.Add(faq);
            await _db.SaveChangesAsync(ct);
            return faq.Id;
        }

        public Task<Faq?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.Faqs.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task UpdateAsync(Faq faq, CancellationToken ct) =>
            _db.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _db.Faqs.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
            return affected > 0;
        }
    }
}
