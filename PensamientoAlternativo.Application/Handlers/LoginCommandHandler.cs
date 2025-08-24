using MediatR;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Application.DTOs.AuthDTOs;
using PensamientoAlternativo.Application.Helpers;
using PensamientoAlternativo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginCommandHandler(ILoginRepository loginRepository, IJwtTokenGenerator tokenGenerator)
        {
            _loginRepository = loginRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Normaliza email
            var email = request.Email.Trim().ToLowerInvariant();

            // 1) Trae el hash guardado (bcrypt: comienza con $2a$ / $2b$ / $2y$)
            var storedHash = await _loginRepository.GetPasswordHashByEmailAsync(email);
            if (string.IsNullOrWhiteSpace(storedHash))
                return null; // usuario no existe

            // 2) Verifica bcrypt (compatible con crypt('..', gen_salt('bf',12)))
            var ok = BCrypt.Net.BCrypt.Verify(request.Password, storedHash);
            if (!ok) return null;

            // 3) JWT
            var userId = await _loginRepository.GetUserIdByEmailAsync(email) ?? email;
            var token = _tokenGenerator.GenerateToken(email, userId);

            return new LoginResponse { Token = token };
        }
    }
}
