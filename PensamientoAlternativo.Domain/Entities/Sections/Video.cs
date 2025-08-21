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
        public bool IsVisible { get; private set; } = false;

        private Video() { }

        public Video(string title, string description, string url,bool isVisible)
        {
            Title = title;
            Description = description;
            Url = url;
            IsVisible = isVisible;
        }

        public void Update(string? title, string? description, bool? isVisible)
        {
            if (title != null) Title = title.Trim();
            if (description != null) Description = description.Trim();
            if (isVisible.HasValue) IsVisible = isVisible.Value;
        }

        public void SetUrl(string url) => Url = url;
    }

}
