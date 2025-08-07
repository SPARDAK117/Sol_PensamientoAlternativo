using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections
{
    public class Video : Entity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Url { get; private set; } = string.Empty;

        private Video() { }

        public Video(string title, string description, string url)
        {
            Title = title;
            Description = description;
            Url = url;
        }
    }

}
