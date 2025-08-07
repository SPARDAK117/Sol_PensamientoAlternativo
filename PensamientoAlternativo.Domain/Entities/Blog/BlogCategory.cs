using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Blog
{
    public class BlogCategory : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public ICollection<Article> Articles { get; private set; } = new List<Article>();

        private BlogCategory() { }

        public BlogCategory(string name)
        {
            Name = name;
        }
    }

}
