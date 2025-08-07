using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.BlogDto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string AuthorName { get; set; } = "Pensamiento Alternativo";
        public DateTime CreatedDate { get; set; }
    }
}
