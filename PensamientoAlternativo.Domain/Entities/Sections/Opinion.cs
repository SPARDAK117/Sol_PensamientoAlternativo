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

        private Opinion() { }

        public Opinion(string authorName, DateTime createdDate, int starRate, string opinion)
        {
            AuthorName = authorName;
            CreatedDate = createdDate;
            StarRate = starRate;
            OpinionText = opinion;
        }
    }

}
