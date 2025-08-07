using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task SendNotificationAsync(ContactForm form, ClientSettings client)
        {
            IConfiguration section = _configuration.GetSection("EmailSettings");

            string? fromEmail = section["FromEmail"];
            string? fromPassword = section["FromPassword"];
            string? smtpHost = section["SmtpHost"];
            string? smtpPortString = section["SmtpPort"];

            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(fromPassword) || string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPortString))
            {
                throw new InvalidOperationException("Email settings are not properly configured.");
            }

            int smtpPort = int.Parse(smtpPortString);
            string body = $@"
                          <h2>Nuevo mensaje de contacto</h2>
                          <p><strong>Nombre:</strong> {form.Customer.Name}</p>
                          <p><strong>Correo:</strong> {form.Customer.Email}</p>
                          <p><strong>Mensaje:</strong><br>{form.Message}</p>
                          <p><strong>Teléfono:</strong> {form.Customer.Phone}</p>
                          <br><hr>
                          <p style='font-size: 12px; color: gray;'>Este mensaje fue enviado desde el sitio web de Pensamiento Alternativo.</p>
                          ";
            MailMessage mail = new()
            {
                From = new MailAddress(fromEmail, "Contacto Pensamiento Alternativo"),
                Subject = $"Nuevo mensaje de {form.Customer.Name}",
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(client.ContactEmail);

            using SmtpClient smtp = new(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new EmailSendException("Error al enviar el correo", ex);
            }
        }
    }

    public class EmailSendException : Exception
    {
        public EmailSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
