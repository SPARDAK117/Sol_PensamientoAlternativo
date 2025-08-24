using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Interfaces
{
    public interface ILoginRepository
    {
        Task<string?> GetPasswordHashByEmailAsync(string email);
        Task<string?> GetUserIdByEmailAsync(string email);
        Task<bool> UpdatePasswordHashByEmailAsync(string email, string upgraded);
    }
}
