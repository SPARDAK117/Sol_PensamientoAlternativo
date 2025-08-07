using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities;

namespace PensamientoAlternativo.Domain.Interfaces;

public interface IClientSettingsRepository : IRepository<ClientSettings>
{
    Task<ClientSettings?> GetActiveSettingsAsync();
}
