using MediatR;
using PensamientoAlternativo.Application.Commands.FaqCommands;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.FaqHandlers
{
    public sealed class CreateFaqHandler : IRequestHandler<CreateFaqCommand, int>
    {
        private readonly IFaqWriteRepository _repo;

        public CreateFaqHandler(IFaqWriteRepository repo) => _repo = repo;

        public async Task<int> Handle(CreateFaqCommand req, CancellationToken ct)
        {
            // Si NO quieres HTML: guarda como texto plano
            var faq = new Faq(
                question: req.Question.Trim(),
                answer: req.Answer.Trim(),
                isVisible: req.IsVisible
            );

            // Si quisieras permitir HTML en Answer, aquí podrías sanitizar con Ganss.XSS antes de guardar.

            return await _repo.CreateAsync(faq, ct);
        }
    }
}
