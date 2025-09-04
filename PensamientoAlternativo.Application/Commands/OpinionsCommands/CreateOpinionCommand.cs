using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PensamientoAlternativo.Application.Commands.OpinionsCommands
{
    public sealed record CreateOpinionCommand(string AuthorName,string Email, int StarRate, string OpinionText, string OpinionText2, string OpinionText3, bool IsVisible,bool AcceptTermsAndConditions,bool EmailNotifications) : IRequest<int>;

}
