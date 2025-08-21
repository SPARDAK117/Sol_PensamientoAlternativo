using Ganss.Xss;
using MediatR;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.BlogHandlers
{
    public sealed class PatchArticleHandler : IRequestHandler<PatchArticleCommand, bool>
    {
        private readonly IArticleWriteRepository _repo;
        private readonly HtmlSanitizer _sanitizer = new();

        public PatchArticleHandler(IArticleWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(PatchArticleCommand req, CancellationToken ct)
        {
            var article = await _repo.GetByIdAsync(req.Id, ct);
            if (article is null) return false;

            if (req.CategoryId is int catId)
            {
                var exists = await _repo.CategoryExistsAsync(catId, ct);
                if (!exists) throw new InvalidOperationException($"Category {catId} not found.");
            }

            article.UpdateBasics(
                title: req.Title,
                description: req.Description,
                imageUrl: req.ImageUrl,
                categoryId: req.CategoryId
            );

            if (req.ContentHtml is not null)
            {
                var safe = _sanitizer.Sanitize(req.ContentHtml);
                article.SetContentHtml(safe);
            }

            await _repo.UpdateAsync(article, ct);
            return true;
        }
    }
}
