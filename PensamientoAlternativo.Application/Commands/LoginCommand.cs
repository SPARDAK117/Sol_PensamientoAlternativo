using MediatR;
using PensamientoAlternativo.Application.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponse?>
    {
        public string Email { get; set; } = "null";
        public string Password { get; set; } = "null";
    }
}
