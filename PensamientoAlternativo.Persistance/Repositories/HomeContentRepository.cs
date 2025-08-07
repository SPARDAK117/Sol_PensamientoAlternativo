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
    public class HomeContentRepository : IHomeContentRepository
    {
        private readonly AppDbContext _context;

        public HomeContentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Image>> GetBannersAsync(CancellationToken ct) => _context.Images.Where(img => img.IsBannerImage).ToListAsync(ct);
        public Task<List<Service>> GetServicesAsync(CancellationToken ct) => _context.Services.ToListAsync(ct);
        public Task<List<Video>> GetVideosAsync(CancellationToken ct) => _context.Videos.ToListAsync(ct);
        public Task<List<Image>> GetImagesAsync(CancellationToken ct) => _context.Images.Where(img => !img.IsBannerImage).ToListAsync(ct);
        public Task<List<Article>> GetArticlesAsync(CancellationToken ct) => _context.Articles.ToListAsync(ct);
        public Task<List<Opinion>> GetOpinionsAsync(CancellationToken ct) => _context.Opinions.ToListAsync(ct);
        public Task<List<Faq>> GetFaqsAsync(CancellationToken ct) => _context.Faqs.ToListAsync(ct);
    }
}
