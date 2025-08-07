using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Persistance;

namespace PensamientoAlternativo.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> FindByEmailAsync(string email)
    {
        return await _context.Customers
            .Include(c => c.ContactForms)
            .FirstOrDefaultAsync(c => c.Email == email);
    }
}
