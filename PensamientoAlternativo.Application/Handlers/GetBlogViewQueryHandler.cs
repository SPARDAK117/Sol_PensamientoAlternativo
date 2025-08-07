using MediatR;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Application.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers
{
    public class GetBlogViewQueryHandler : IRequestHandler<GetBlogViewQuery, BlogViewDto>
    {
        private readonly IBlogRepository _repo;

        public GetBlogViewQueryHandler(IBlogRepository repo)
        {
            _repo = repo;
        }

        public async Task<BlogViewDto> Handle(GetBlogViewQuery request, CancellationToken ct)
        {
            var categories = await _repo.GetCategoriesAsync(ct);
            var topArticles = await _repo.GetTopArticlesAsync(ct);
            var (latestArticles, totalCount) = await _repo.GetLatestArticlesAsync(1, 8, ct);

            var totalPages = (int)Math.Ceiling((double)totalCount / 8);
            return new BlogViewDto
            {
                View = "Blog",
                Sections = new List<BlogSectionDto>
            {
                new()
                    {
                     IdSection = 1,
                     Section = "Categorias",
                     Content = categories
                    },
                new()
                    {
                     IdSection = 2,
                     Section = "Top Articles",
                     Content = topArticles
                    },
                new()
                    {
                     IdSection = 3,
                     Section = "Listado inicial",
                     Content = new
                     {
                         Articles = latestArticles,
                         Pagination = new PaginationDto
                         {
                             Page = 1,
                             PageSize = 8,
                             TotalPages = totalPages,
                             TotalItems = totalCount
                         }
                     }
            }
            }
            };
        }
    }

}
