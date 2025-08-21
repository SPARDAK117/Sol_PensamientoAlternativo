using MediatR;
using PensamientoAlternativo.Application.Commands.FaqCommands;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.FaqHandlers
{
    public sealed class PatchFaqHandler : IRequestHandler<PatchFaqCommand, bool>
    {
        private readonly IFaqWriteRepository _repo;
        public PatchFaqHandler(IFaqWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(PatchFaqCommand req, CancellationToken ct)
        {
            var faq = await _repo.GetByIdAsync(req.Id, ct);
            if (faq is null) return false;

            faq.Update(req.Question, req.Answer,req.IsVisible);
            await _repo.UpdateAsync(faq, ct);
            return true;
        }
    }
}
