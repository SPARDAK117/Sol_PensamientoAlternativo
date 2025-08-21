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
        public bool IsVisible { get; private set; } = false;

        private Faq() { }

        public void Update(string? question, string? answer,bool isVisible)
        {
            if (question != null) Question = question.Trim();
            if (answer != null) Answer = answer.Trim();
            if (!isVisible) Answer = string.Empty;
        }

        public Faq(string question, string answer,bool isVisible)
        {
            Question = question;
            Answer = answer;
            IsVisible = isVisible;
        }
    }

}
