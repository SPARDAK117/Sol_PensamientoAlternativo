using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands.FaqCommands
{
    public sealed record DeleteFaqCommand(int Id) : IRequest<bool>;
}
