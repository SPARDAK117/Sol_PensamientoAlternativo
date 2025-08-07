using PensamientoAlternativo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendNotificationAsync(ContactForm form, ClientSettings client);
    }
}
