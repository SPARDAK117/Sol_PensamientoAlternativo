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
    public class ImageWriteRepository : IImageWriteRepository
    {
        private readonly AppDbContext _context;

        public ImageWriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Image?> GetByIdAsync(int id, CancellationToken ct) =>
        _context.Images.FirstOrDefaultAsync(a => a.Id == id, ct);

        public Task<bool> ImageExistsAsync(int imageId, CancellationToken ct) =>
        _context.Images.AnyAsync(c => c.Id == imageId, ct);

        public async Task<int> CreateAsync(Image image, CancellationToken ct)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync(ct);
            return image.Id;
        }

        public Task UpdateAsync(Image image, CancellationToken ct) =>
            _context.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _context.Images
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }


    }
}
