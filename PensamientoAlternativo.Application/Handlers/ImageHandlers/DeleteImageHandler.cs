using MediatR;
using PensamientoAlternativo.Application.Commands.ImageCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;

namespace PensamientoAlternativo.Application.Handlers.ImageHandlers
{

    public sealed class DeleteImageHandler : IRequestHandler<DeleteImageCommand, bool>
    {
        private readonly IImageWriteRepository _repo;
        private readonly IImageStorage _storage;

        public DeleteImageHandler(IImageWriteRepository repo, IImageStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<bool> Handle(DeleteImageCommand request, CancellationToken ct)
        {
            Image img = await _repo.GetByIdAsync(request.Id, ct);
            if (img is null) return false;

            await _storage.DeleteByPublicUrlAsync(img.Url, ct);

            return await _repo.DeleteAsync(request.Id, ct);
        }
    }

}
