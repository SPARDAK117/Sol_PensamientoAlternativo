using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string email, string userId);
    }
}
