using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.ImageDTOs
{
    public sealed class CreateImageFormDto
    {
        public bool IsBannerImage { get; set; }
        public bool IsVisible { get; set; } = true;
        public int ViewSection { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile File { get; set; } = default!;
    }
}
