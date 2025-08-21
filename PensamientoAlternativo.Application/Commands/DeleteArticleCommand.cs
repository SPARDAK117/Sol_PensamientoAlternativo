using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands
{
    public sealed record DeleteArticleCommand(int Id) : IRequest<bool>;
}
