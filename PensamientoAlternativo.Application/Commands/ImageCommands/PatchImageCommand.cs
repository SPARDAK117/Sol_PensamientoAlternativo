using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.ImageCommands
{
    public sealed record PatchImageCommand(
        int Id,
        string? Title,
        string? Description,
        bool? IsBannerImage,
        bool? IsActive,
        Stream? Content,
        string? ContentType,
        string? OriginalFileName
    ) : IRequest<bool>;
}
