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
    public sealed class VideoWriteRepository : IVideoWriteRepository
    {
        private readonly AppDbContext _db;
        public VideoWriteRepository(AppDbContext db) => _db = db;

        public async Task<int> CreateAsync(Video video, CancellationToken ct)
        {
            _db.Videos.Add(video);
            await _db.SaveChangesAsync(ct);
            return video.Id;
        }

        public Task<Video?> GetByIdAsync(int id, CancellationToken ct) =>
            _db.Videos.FirstOrDefaultAsync(v => v.Id == id, ct);

        public Task UpdateAsync(Video video, CancellationToken ct) =>
            _db.SaveChangesAsync(ct);

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var affected = await _db.Videos.Where(v => v.Id == id).ExecuteDeleteAsync(ct);
            return affected > 0;
        }
    }
}
