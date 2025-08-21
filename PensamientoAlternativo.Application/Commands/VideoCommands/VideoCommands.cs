using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace PensamientoAlternativo.Application.Commands.VideoCommands
{

    public sealed record CreateVideoCommand(
        string Title,
        string Description,
        bool IsVisible,
        int viewSection,
        Stream Content,
        string ContentType,
        string OriginalFileName
    ) : IRequest<int>;

    public sealed record PatchVideoCommand(
        int Id,
        string? Title,
        string? Description,
        bool? IsVisible,
        Stream? Content,
        string? ContentType,
        string? OriginalFileName
    ) : IRequest<bool>;

    public sealed record DeleteVideoCommand(int Id) : IRequest<bool>;

}
