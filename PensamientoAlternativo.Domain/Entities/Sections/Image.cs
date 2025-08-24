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
        public bool IsBannerImage { get; set; } = false;
        public int ViewSection { get; set; }
        public bool IsVisible { get; set; } = false;
        public string Title { get; private set; } = string.Empty;
        public string Url { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public Image(bool isBannerImage, bool isVisible,int viewSection, string title, string url, string description)
        {
            IsBannerImage = isBannerImage;
            IsVisible = isVisible;
            ViewSection = viewSection;
            Title = title ?? string.Empty;
            Url = url ?? string.Empty;
            Description = description ?? string.Empty;
        }

        public void UpdateMetadata(string? title, string? description, bool? isBanner, bool? isActive)
        {
            if (title != null) Title = title.Trim();
            if (description != null) Description = description.Trim();
            if (isBanner.HasValue) IsBannerImage = isBanner.Value;
            if (isActive.HasValue) IsVisible = isActive.Value;
        }

        public void SetPath(string newPublicUrl)
        {
            Url  = newPublicUrl ?? Url;
        }
    }

}
