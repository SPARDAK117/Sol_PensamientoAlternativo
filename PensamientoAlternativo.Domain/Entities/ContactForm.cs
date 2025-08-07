using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities
{
    public class ContactForm : Entity
    {
        public string Message { get; private set; } = string.Empty;
        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        private ContactForm() { } 

        public ContactForm(Customer customer, string message)
        {
            Customer = customer;
            CustomerId = customer.Id;
            Message = message;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
