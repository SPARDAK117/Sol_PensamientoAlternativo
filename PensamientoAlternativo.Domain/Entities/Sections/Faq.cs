using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections
{
    public class Faq : Entity
    {
        public string Question { get; private set; } = string.Empty;
        public string Answer { get; private set; } = string.Empty;

        private Faq() { }

        public Faq(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }

}
