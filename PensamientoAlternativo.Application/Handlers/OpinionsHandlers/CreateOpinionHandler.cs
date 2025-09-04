using AngleSharp.Io;
using MediatR;
using PensamientoAlternativo.Application.Commands.OpinionsCommands;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.OpinionsHandlers
{
    public sealed class CreateOpinionHandler(IOpinionWriteRepository repo, ICustomerRepository customerRepository) : IRequestHandler<CreateOpinionCommand, int>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository
;
        private readonly IOpinionWriteRepository _repo = repo;

        public async Task<int> Handle(CreateOpinionCommand req, CancellationToken cancellationToken)
        {
            var op = new Opinion(
                authorName: req.AuthorName.Trim(),
                createdDate: DateTime.UtcNow,
                starRate: req.StarRate,
                opinion: req.OpinionText.Trim(),
                isVisible: req.IsVisible,
                opinion2: req.OpinionText2.Trim(),
                opinion3: req.OpinionText3.Trim()
            );


            if (!string.IsNullOrEmpty(req.Email))
            {
                Customer? customer = await _customerRepository.FindByEmailAsync(req.Email);
                if (customer is null)
                {
                    customer = new Customer(req.AuthorName, req.Email, req.EmailNotifications, req.AcceptTermsAndConditions);
                    await _customerRepository.AddAsync(customer);
                }
            }

            return await _repo.CreateAsync(op, cancellationToken);
        }
    }
}
