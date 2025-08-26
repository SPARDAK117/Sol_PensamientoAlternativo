using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.ServiceDTOs
{ 

    public class ServiceRequestDto
    {
        public string? IconName { get;  set; } = string.Empty;
        public string? IconPath { get;  set; } = string.Empty;
        public string? Title { get;  set; } = string.Empty;
        public string? Subtitle { get;  set; } = string.Empty;
    }
}