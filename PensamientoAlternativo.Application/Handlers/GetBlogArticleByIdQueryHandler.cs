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
    public class GetBlogArticleByIdQueryHandler : IRequestHandler<GetBlogArticleByIdQuery, BlogArticleDetailDto?>
    {
        private readonly IBlogRepository _repository;

        public GetBlogArticleByIdQueryHandler(IBlogRepository repository)
        {
            _repository = repository;
        }

        public async Task<BlogArticleDetailDto?> Handle(GetBlogArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetArticleByIdAsync(request.ArticleId);
        }
    }
}
