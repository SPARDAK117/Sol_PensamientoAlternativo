using MediatR;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Domain.Entities.Blog;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.CategoriesHandler
{
    public sealed class CreateBlogCategoryHandler : IRequestHandler<CreateBlogCategoryCommand, int>
    {
        private readonly IBlogCategoryWriteRepository _repo;
        public CreateBlogCategoryHandler(IBlogCategoryWriteRepository repo) => _repo = repo;

        public async Task<int> Handle(CreateBlogCategoryCommand req, CancellationToken ct)
        {
            if (await _repo.ExistsByNameAsync(req.Name.Trim(), excludeId: null, ct))
                throw new InvalidOperationException($"La categoría '{req.Name}' ya existe.");

            var cat = new BlogCategory(req.Name);
            return await _repo.CreateAsync(cat, ct);
        }
    }

    public sealed class PatchBlogCategoryHandler : IRequestHandler<PatchBlogCategoryCommand, bool>
    {
        private readonly IBlogCategoryWriteRepository _repo;
        public PatchBlogCategoryHandler(IBlogCategoryWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(PatchBlogCategoryCommand req, CancellationToken ct)
        {
            var cat = await _repo.GetByIdAsync(req.Id, ct);
            if (cat is null) return false;

            if (await _repo.ExistsByNameAsync(req.Name.Trim(), excludeId: req.Id, ct))
                throw new InvalidOperationException($"La categoría '{req.Name}' ya existe.");

            cat.Rename(req.Name);
            await _repo.UpdateAsync(cat, ct);
            return true;
        }
    }

    public sealed class DeleteBlogCategoryHandler : IRequestHandler<DeleteBlogCategoryCommand, bool>
    {
        private readonly IBlogCategoryWriteRepository _repo;
        public DeleteBlogCategoryHandler(IBlogCategoryWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteBlogCategoryCommand req, CancellationToken ct)
        {
            if (await _repo.HasArticlesAsync(req.Id, ct))
                throw new InvalidOperationException("No puede eliminarse: tiene artículos asociados.");

            return await _repo.DeleteAsync(req.Id, ct);
        }
    }
}
