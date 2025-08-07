using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Persistance;

namespace PensamientoAlternativo.Infrastructure.Repositories;

public class ContactFormRepository : Repository<ContactForm>, IContactFormRepository
{
    public ContactFormRepository(AppDbContext context) : base(context) { }

    public async Task DeleteAsync(int id)
    {
        var form = await _context.ContactForms.FindAsync(id);
        if (form is not null)
        {
            _context.ContactForms.Remove(form);
        }
    }
}
