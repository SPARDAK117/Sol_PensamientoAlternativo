using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections;

public class Service : Entity
{
    public string IconName { get;  set; } = string.Empty;
    public string IconPath { get;  set; } = string.Empty;
    public string Title { get;  set; } = string.Empty;
    public string Subtitle { get;  set; } = string.Empty;

    public Service() { }

    public Service(string iconName, string iconPath, string title, string subtitle)
    {
        IconName = iconName;
        IconPath = iconPath;
        Title = title;
        Subtitle = subtitle;
    }

    public void UpdateMetadata(string title, string subtitle, string iconPath, string iconName)
    {
        if(!string.IsNullOrEmpty(title)) Title = title.Trim();
        if (!string.IsNullOrEmpty(subtitle)) Subtitle = subtitle.Trim();
        if (!string.IsNullOrEmpty(iconPath)) IconName = iconName.Trim();
        if (!string.IsNullOrEmpty(iconName)) IconPath = iconPath.Trim();

    }
}

