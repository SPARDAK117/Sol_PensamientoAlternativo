using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections
{
    public class Image : Entity
    {
        public string Title { get; private set; } = string.Empty;
        public string Path { get; private set; } = string.Empty;

        private Image() { }

        public Image(string title, string path)
        {
            Title = title;
            Path = path;
        }
    }

}
