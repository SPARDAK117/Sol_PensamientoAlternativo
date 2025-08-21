using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.FaqDTOs
{
    public sealed class UpdateFaqRequest
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public bool IsVisible { get; set; } = false;

    }
}
