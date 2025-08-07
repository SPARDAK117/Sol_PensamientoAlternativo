using PensamientoAlternativo.Application.DTOs.BlogDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Interfaces
{
    public interface IBlogRepository
    {
        Task<List<string>> GetCategoriesAsync(CancellationToken ct);
        Task<List<ArticleDto>> GetTopArticlesAsync(CancellationToken ct);
        Task<(List<ArticleDto> articles, int totalCount)> GetLatestArticlesAsync(int page, int pageSize, CancellationToken ct);
        Task<(List<ArticleDto>, int)> GetArticlesByCategoryAsync(string? category, int page, int pageSize, CancellationToken ct);
        Task<BlogArticleDetailDto?> GetArticleByIdAsync(int articleId);

    }
}
