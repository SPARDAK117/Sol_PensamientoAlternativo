using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities
{
    public class ClientSettings : Entity
    {
        [Required]
        public string ClientName { get; private set; } = string.Empty;
        [Required]
        public string ContactEmail { get; private set; } = string.Empty;
        [Required]
        public string WhatsAppNumber { get; private set; } = string.Empty;
        [Required]
        public bool IsActive { get; private set; } = true;
        [Required]
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private ClientSettings() { }

        public ClientSettings(string clientName, string contactEmail, string whatsappNumber)
        {
            ClientName = clientName;
            ContactEmail = contactEmail;
            WhatsAppNumber = whatsappNumber;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
