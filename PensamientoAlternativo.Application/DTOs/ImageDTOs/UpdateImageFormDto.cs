using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.ImageDTOs
{
    public sealed class UpdateImageFormDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsBannerImage { get; set; }
        public bool? IsActive { get; set; }
        public IFormFile? File { get; set; } // opcional: nuevo archivo
    }
}
