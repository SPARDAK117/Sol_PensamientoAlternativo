using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities
{
    public class Customer : AggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;

        public List<ContactForm> ContactForms { get; private set; } = new();

        private Customer() { } 

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public void Update(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public ContactForm AddContactForm(string message)
        {
            ContactForm form = new ContactForm(this, message);
            ContactForms.Add(form);
            return form;
        }
    }
}
