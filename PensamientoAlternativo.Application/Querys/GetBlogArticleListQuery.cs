using MediatR;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Querys
{
    public class GetBlogArticleListQuery : IRequest<ArticleListResultDto>
    {
        public int Page { get; }
        public int PageSize { get; }
        public string? Category { get; }

        public GetBlogArticleListQuery(int page, int pageSize, string? category)
        {
            Page = page;
            PageSize = pageSize;
            Category = category;
        }
    }

}
