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
    public sealed class CreateOpinionHandler : IRequestHandler<CreateOpinionCommand, int>
    {
        private readonly IOpinionWriteRepository _repo;
        public CreateOpinionHandler(IOpinionWriteRepository repo) => _repo = repo;

        public async Task<int> Handle(CreateOpinionCommand req, CancellationToken ct)
        {
            var op = new Opinion(
                authorName: req.AuthorName.Trim(),
                createdDate: DateTime.UtcNow,
                starRate: req.StarRate,
                opinion: req.OpinionText.Trim(),
                isVisible: req.IsVisible
            );

            return await _repo.CreateAsync(op, ct);
        }
    }
}
