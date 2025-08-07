using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.BlogDto
{
    public class ArticleListResultDto
    {
        public List<ArticleDto> Articles { get; set; } = [];
        public PaginationDto Pagination { get; set; } = new();
    }
}
