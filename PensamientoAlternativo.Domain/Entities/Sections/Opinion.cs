using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections
{
    public class Opinion : Entity
    {
        public string AuthorName { get; private set; } = string.Empty;
        public DateTime CreatedDate { get; private set; }
        public int StarRate { get; private set; }
        public string OpinionText { get; private set; } = string.Empty;
        public bool IsVisible { get; private set; } = false;


        private Opinion() { }

        public Opinion(string authorName, DateTime createdDate, int starRate, string opinion,bool isVisible)
        {
            AuthorName = authorName;
            CreatedDate = createdDate;
            StarRate = starRate;
            OpinionText = opinion;
            IsVisible = isVisible;
        }
        public void Update(string? authorName, int? starRate, string? opinionText,bool isVisible)
        {
            if (authorName is not null) AuthorName = authorName.Trim();
            if (starRate is not null) StarRate = starRate.Value;     
            if (opinionText is not null) OpinionText = opinionText.Trim();
            if (isVisible) IsVisible = isVisible;
        }
    }

}
