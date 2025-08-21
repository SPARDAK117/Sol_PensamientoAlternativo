using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.ImageCommands
{
    public sealed record CreateImageCommand(
        bool IsBannerImage,
        bool IsVisible,
        int ViewSection,
        string Title,
        string Description,
        Stream Content,
        string ContentType,
        string OriginalFileName
    ) : IRequest<int>;

}