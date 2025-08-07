using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PensamientoAlternativo.Persistance.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetCategoriesAsync(CancellationToken ct)
        {
            return await _context.BlogCategories
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<List<ArticleDto>> GetTopArticlesAsync(CancellationToken ct)
        {
            return await _context.Articles
                .OrderByDescending(a => a.CreatedDate)
                .Take(5)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Introduction = a.Description,
                    ImageUrl = a.ImageUrl,
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync(ct);
        }

        public async Task<(List<ArticleDto>, int)> GetLatestArticlesAsync(int page, int pageSize, CancellationToken ct)
        {
            var query = _context.Articles.OrderByDescending(a => a.CreatedDate);

            var totalCount = await query.CountAsync(ct);
            var articles = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Introduction = a.Description,
                    ImageUrl = a.ImageUrl,
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync(ct);

            return (articles, totalCount);
        }
        public async Task<(List<ArticleDto>, int)> GetArticlesByCategoryAsync(string? category, int page, int pageSize, CancellationToken ct)
        {
            var query = _context.Articles
                .AsQueryable()
                .OrderByDescending(a => a.CreatedDate);

            if (!string.IsNullOrWhiteSpace(category))
                query = (IOrderedQueryable<Domain.Entities.Sections.Article>)query.Where(a => a.Category.Name == category);

            var totalCount = await query.CountAsync(ct);

            var articles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Introduction = a.Description,
                    ImageUrl = a.ImageUrl,
                    AuthorName = a.AuthorName,
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync(ct);

            return (articles, totalCount);
        }
        public async Task<BlogArticleDetailDto?> GetArticleByIdAsync(int articleId)
        {
            return await _context.Articles
                .Where(x => x.Id == articleId)
                .Select(x => new BlogArticleDetailDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    AuthorName = x.AuthorName,
                    CreatedDate = x.CreatedDate,
                    ContentHtml = x.ContentHtml
                })
                .FirstOrDefaultAsync();
        }
    }

}
