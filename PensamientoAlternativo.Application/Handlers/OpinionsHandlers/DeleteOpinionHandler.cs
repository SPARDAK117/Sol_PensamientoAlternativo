using MediatR;
using PensamientoAlternativo.Application.Commands.OpinionsCommands;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.OpinionsHandlers
{
    public sealed class DeleteOpinionHandler : IRequestHandler<DeleteOpinionCommand, bool>
    {
        private readonly IOpinionWriteRepository _repo;
        public DeleteOpinionHandler(IOpinionWriteRepository repo) => _repo = repo;

        public Task<bool> Handle(DeleteOpinionCommand req, CancellationToken ct) =>
            _repo.DeleteAsync(req.Id, ct);
    }
}
