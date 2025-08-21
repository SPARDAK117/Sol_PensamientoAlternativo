using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.CategoriesDTOs
{
    public sealed class UpdateBlogCategoryRequest
    {
        public string Name { get; set; } = string.Empty;  // único campo
    }
}
