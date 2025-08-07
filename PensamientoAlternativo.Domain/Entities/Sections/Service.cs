using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Sections;

public class Service : Entity
{
    public string IconName { get; private set; } = string.Empty;
    public string IconPath { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Subtitle { get; private set; } = string.Empty;

    private Service() { }

    public Service(string iconName, string iconPath, string title, string subtitle)
    {
        IconName = iconName;
        IconPath = iconPath;
        Title = title;
        Subtitle = subtitle;
    }
}

