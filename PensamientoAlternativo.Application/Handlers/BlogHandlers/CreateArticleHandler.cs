using Ganss.Xss;
using MediatR;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;

namespace PensamientoAlternativo.Application.Handlers.BlogHandlers
{
    public sealed class CreateArticleHandler : IRequestHandler<CreateArticleCommand, int>
    {
        private readonly IArticleWriteRepository _repo;
        private readonly HtmlSanitizer _sanitizer = new();

        public CreateArticleHandler(IArticleWriteRepository repo)
        {
            _repo = repo;
            // Configura si tu editor requiere cosas extra:
            // _sanitizer.AllowedTags.Add("iframe");
            // _sanitizer.AllowedAttributes.Add("class");
        }

        public async Task<int> Handle(CreateArticleCommand request, CancellationToken ct)
        {
            var exists = await _repo.CategoryExistsAsync(request.CategoryId, ct);
            if (!exists) throw new InvalidOperationException($"Category {request.CategoryId} not found.");

            var article = new Article(
                title: request.Title.Trim(),
                description: request.Description.Trim(),
                imageUrl: request.ImageUrl.Trim(),
                categoryId: request.CategoryId
            );

            article.SetContentHtml(_sanitizer.Sanitize(request.ContentHtml ?? string.Empty));

            return await _repo.CreateAsync(article, ct);
        }
    }
}
