using MediatR;
using PensamientoAlternativo.Application.Commands.VideoCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.VideoHandlers
{
    public sealed class DeleteVideoHandler : IRequestHandler<DeleteVideoCommand, bool>
    {
        private readonly IVideoWriteRepository _repo;
        private readonly IImageStorage _storage;

        public DeleteVideoHandler(IVideoWriteRepository repo, IImageStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<bool> Handle(DeleteVideoCommand req, CancellationToken ct)
        {
            var v = await _repo.GetByIdAsync(req.Id, ct);
            if (v is null) return false;

            // Borra del bucket primero (idempotente)
            try { await _storage.DeleteByPublicUrlAsync(v.Url, ct); } catch { return false; }

            // Luego borra registro
            return await _repo.DeleteAsync(req.Id, ct);
        }
    }
}
