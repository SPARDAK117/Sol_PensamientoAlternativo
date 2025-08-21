using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands
{
    public sealed record CreateBlogCategoryCommand(string Name) : IRequest<int>;
    public sealed record PatchBlogCategoryCommand(int Id, string Name) : IRequest<bool>;
    public sealed record DeleteBlogCategoryCommand(int Id) : IRequest<bool>;
}
