using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;

namespace PensamientoAlternativo.Persistance.Repositories;

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
