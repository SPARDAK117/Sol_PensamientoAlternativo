using FluentValidation;
using PensamientoAlternativo.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Blog
{
    public sealed class CreateArticleValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(500);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            // ContentHtml puede ser vacío, pero normalmente tendrás algo
        }
    }
}
