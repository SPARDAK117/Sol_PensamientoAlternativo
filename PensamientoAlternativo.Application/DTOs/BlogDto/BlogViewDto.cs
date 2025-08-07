using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.BlogDto
{
    public class BlogViewDto
    {
        public string View { get; set; } = "Blog";
        public List<BlogSectionDto> Sections { get; set; } = [];
    }
}
