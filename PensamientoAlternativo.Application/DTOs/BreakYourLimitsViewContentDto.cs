using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs
{
    public class BreakYourLimitsViewContentDto
    {
        public string View { get; set; } = string.Empty;
        public List<SectionBreakYourLimitsDto> Sections { get; set; } = [];
    }
}
