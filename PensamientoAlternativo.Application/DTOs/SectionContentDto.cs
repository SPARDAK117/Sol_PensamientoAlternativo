using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs
{
    public class SectionContentDto
    {
        public int IdSection { get; set; }

        public string Section { get; set; } = string.Empty;

        public object? Content { get; set; }
    }
}
