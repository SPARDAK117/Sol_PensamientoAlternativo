using MediatR;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Querys
{
    public class GetBlogArticleByIdQuery : IRequest<BlogArticleDetailDto?>
    {
        public int ArticleId { get; set; }

        public GetBlogArticleByIdQuery(int articleId)
        {
            ArticleId = articleId;
        }
    }
}
