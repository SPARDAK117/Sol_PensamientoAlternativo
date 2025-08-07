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
    public class GetBlogArticleListQueryHandler : IRequestHandler<GetBlogArticleListQuery, ArticleListResultDto>
    {
        private readonly IBlogRepository _repo;

        public GetBlogArticleListQueryHandler(IBlogRepository repo)
        {
            _repo = repo;
        }

        public async Task<ArticleListResultDto> Handle(GetBlogArticleListQuery request, CancellationToken cancellationToken)
        {
            var (articles, totalCount) = await _repo.GetArticlesByCategoryAsync(request.Category, request.Page, request.PageSize, cancellationToken);

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            return new ArticleListResultDto
            {
                Articles = articles,
                Pagination = new PaginationDto
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = totalCount,
                    TotalPages = totalPages
                }
            };
        }
    }

}
