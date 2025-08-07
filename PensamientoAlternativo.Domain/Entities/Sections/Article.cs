using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections
{
    public class Article : Entity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public string AuthorName { get; private set; } = "Pensamiento Alternativo";
        public DateTime CreatedDate { get; private set; }

        public string ContentHtml { get; private set; } = string.Empty;

        public int CategoryId { get; private set; }
        public BlogCategory Category { get; private set; } = null!;

        private Article() { }

        public Article(string title, string description, string imageUrl, int categoryId)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            CreatedDate = DateTime.UtcNow;
            AuthorName = "Pensamiento Alternativo";
            CategoryId = categoryId;
        }
    }


}
