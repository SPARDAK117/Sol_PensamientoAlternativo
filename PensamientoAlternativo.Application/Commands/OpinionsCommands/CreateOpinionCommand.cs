using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PensamientoAlternativo.Application.Commands.OpinionsCommands
{
    public sealed record CreateOpinionCommand(string AuthorName, int StarRate, string OpinionText,bool IsVisible) : IRequest<int>;

}
