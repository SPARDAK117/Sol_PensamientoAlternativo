using MediatR;
using PensamientoAlternativo.Application.Commands.OpinionsCommands;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.OpinionsHandlers
{
    public sealed class PatchOpinionHandler : IRequestHandler<PatchOpinionCommand, bool>
    {
        private readonly IOpinionWriteRepository _repo;
        public PatchOpinionHandler(IOpinionWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(PatchOpinionCommand req, CancellationToken ct)
        {
            Opinion op = await _repo.GetByIdAsync(req.Id, ct);
            if (op is null) return false;

            // Validaciones ya pasaron por FluentValidation
            op.Update(req.AuthorName, req.StarRate, req.OpinionText,req.IsVisible);

            await _repo.UpdateAsync(op, ct);
            return true;
        }
    }
}
