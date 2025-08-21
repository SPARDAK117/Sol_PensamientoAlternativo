using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.ImageCommands
{
    public sealed record DeleteImageCommand(int Id) : IRequest<bool>;
}
