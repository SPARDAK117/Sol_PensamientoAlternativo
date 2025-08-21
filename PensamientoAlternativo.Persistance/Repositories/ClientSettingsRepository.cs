using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Domain.Entities;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Persistance;

namespace PensamientoAlternativo.Persistance.Repositories;

public class ClientSettingsRepository : Repository<ClientSettings>, IClientSettingsRepository
{
    public ClientSettingsRepository(AppDbContext context) : base(context) { }

    public async Task<ClientSettings?> GetActiveSettingsAsync()
    {
        return await _context.ClientSettings
            .Where(cs => cs.IsActive)
            .OrderByDescending(cs => cs.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
