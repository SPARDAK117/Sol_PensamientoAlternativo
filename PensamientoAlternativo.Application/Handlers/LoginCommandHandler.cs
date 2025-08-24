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
            string email = request.Email?.Trim().ToLowerInvariant() ?? "";
            if (string.IsNullOrWhiteSpace(email)) return null;

            string storedHash = await _loginRepository.GetPasswordHashByEmailAsync(email) ?? "";
            if (string.IsNullOrWhiteSpace(storedHash))
                return null;

            var ok = PasswordHasher.Verify(request.Password, storedHash);
            if (!ok) return null;

            if (PasswordHasher.NeedsRehash(storedHash))
            {
                var upgraded = PasswordHasher.Hash(request.Password);
                await _loginRepository.UpdatePasswordHashByEmailAsync(email, upgraded);
            }

            string userId = await _loginRepository.GetUserIdByEmailAsync(email) ?? email;
            string token = _tokenGenerator.GenerateToken(email, userId);

            return new LoginResponse { Token = token };
        }
    }
}
