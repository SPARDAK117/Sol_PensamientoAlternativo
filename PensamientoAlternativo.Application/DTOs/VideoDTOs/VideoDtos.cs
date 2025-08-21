using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.VideoDTOs
{
    public sealed class CreateVideoFormDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
        public IFormFile File { get; set; } = default!;
    }

    public sealed class UpdateVideoFormDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsVisible { get; set; }
        public IFormFile? File { get; set; }
    }

}
