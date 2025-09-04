using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.DTOs.OpinionsDTOs
{
    public sealed class CreateOpinionRequest
    {
        public string AuthorName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public int StarRate { get; set; }       
        public string OpinionText { get; set; } = string.Empty;
        public string OpinionText2 { get; set; } = string.Empty;
        public string OpinionText3 { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = false;
        public bool AcceptTermsAndConditions { get; set; } = false;
        public bool EmailNotifications { get; set; } = false;
    }
}
