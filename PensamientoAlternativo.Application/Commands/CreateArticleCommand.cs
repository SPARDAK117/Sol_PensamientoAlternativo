using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands
{
    public sealed record CreateArticleCommand(
        string Title,
        string Description,
        string ImageUrl,
        int CategoryId,
        string ContentHtml
    ) : IRequest<int>;
}
