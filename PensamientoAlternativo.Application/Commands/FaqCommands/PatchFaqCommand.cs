using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.FaqCommands
{
    public sealed record PatchFaqCommand(int Id, string? Question, string? Answer, bool IsVisible) : IRequest<bool>;
}
