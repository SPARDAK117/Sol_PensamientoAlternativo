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
    public sealed class DeleteFaqHandler : IRequestHandler<DeleteFaqCommand, bool>
    {
        private readonly IFaqWriteRepository _repo;
        public DeleteFaqHandler(IFaqWriteRepository repo) => _repo = repo;

        public Task<bool> Handle(DeleteFaqCommand req, CancellationToken ct)
            => _repo.DeleteAsync(req.Id, ct);
    }
}
