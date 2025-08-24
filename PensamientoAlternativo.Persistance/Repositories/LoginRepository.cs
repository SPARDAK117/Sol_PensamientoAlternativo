using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Persistance.Repositories
{
    public sealed class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _db;

        public LoginRepository(AppDbContext db) => _db = db;

        public async Task<string?> GetPasswordHashByEmailAsync(string email)
            => await _db.LoginCredentials
                   .Where(x => x.UserEmail == email)
                   .Select(x => x.Password)
                   .FirstOrDefaultAsync();

        public async Task<string?> GetUserIdByEmailAsync(string email)
            => await _db.LoginCredentials
                   .Where(u => u.UserEmail == email)
                   .Select(u => u.Id.ToString())
                   .FirstOrDefaultAsync();
        public async Task<bool> UpdatePasswordHashByEmailAsync(string email, string upgraded)
        {
            var user = await _db.LoginCredentials
                                .FirstOrDefaultAsync(u => u.UserEmail == email);

            if (user is null)
                return false;

            user.Password = upgraded;

            _db.LoginCredentials.Update(user);
            await _db.SaveChangesAsync();

            return true;
        }

    }

}
