using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.OpinionsCommands
{
    public sealed record DeleteOpinionCommand(int Id) : IRequest<bool>;
}
