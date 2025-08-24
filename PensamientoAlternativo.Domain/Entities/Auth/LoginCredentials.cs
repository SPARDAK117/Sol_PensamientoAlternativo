using Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Entities.Auth
{
    public class LoginCredentials:Entity
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public LoginCredentials(string userEmail, string password) 
        {
            UserEmail = userEmail;
            Password = password;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
        }

    }
}
