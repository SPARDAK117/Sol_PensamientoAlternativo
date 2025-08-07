using Domain.Seedwork;
using MediatR;
using PensamientoAlternativo.Application.Commands.FormCommand;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.FormHandler
{
    public class SubmitContactFormHandler : IRequestHandler<SubmitContactFormCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly IClientSettingsRepository _settingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitContactFormHandler(
            ICustomerRepository customerRepository,
            IEmailService emailService,
            IClientSettingsRepository settingsRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
            _settingsRepository = settingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SubmitContactFormCommand request, CancellationToken cancellationToken)
        {
            ClientSettings? clientSettings = await _settingsRepository.GetActiveSettingsAsync();
            if (clientSettings is null)
                throw new ArgumentNullException($"{clientSettings} is null");

            Customer? customer = await _customerRepository.FindByEmailAsync(request.Email);
            if (customer is null)
            {
                customer = new Customer(request.Name, request.Email, request.Phone);
                await _customerRepository.AddAsync(customer);
            }
            else
            {
                customer.Update(request.Name, request.Phone);
            }

            ContactForm form = customer.AddContactForm(request.Message);

            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendNotificationAsync(form, clientSettings);

            return Unit.Value;
        }
    }


}
