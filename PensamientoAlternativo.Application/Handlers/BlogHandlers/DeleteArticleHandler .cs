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
    public sealed class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, bool>
    {
        private readonly IArticleWriteRepository _repo;

        public DeleteArticleHandler(IArticleWriteRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteArticleCommand request, CancellationToken ct)
            => await _repo.DeleteAsync(request.Id, ct);
    }
}
