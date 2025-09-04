using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public bool IsEmailNotificationsAvailable { get; private set; } = false;
        public bool AcceptTermsAndConditions { get; private set; } = false;

        public List<ContactForm> ContactForms { get; private set; } = new();
        public List<Opinion> Opinions { get; private set; } = new();

        private Customer() { } 

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
        public Customer(string name, string email,bool isEmailNotificationsAvailable,bool acceptTermsAndConditions)
        {
            Name = name;
            Email = email;
            IsEmailNotificationsAvailable = isEmailNotificationsAvailable;
            AcceptTermsAndConditions = acceptTermsAndConditions;
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
        public Opinion AddOpinion(string name,int starRate,string message,string message2,string message3)
        {
            Opinion form = new Opinion(name, DateTime.Now.Date, starRate, message, message2, message3,true);
            Opinions.Add(form);
            return form;
        }

        public void NotificationsAvailable(bool isTrue) 
        {
            IsEmailNotificationsAvailable = isTrue;
        }
        public void TermsAndConditionsAvailable(bool isTrue)
        {
            AcceptTermsAndConditions = isTrue;
        }

    }
}
