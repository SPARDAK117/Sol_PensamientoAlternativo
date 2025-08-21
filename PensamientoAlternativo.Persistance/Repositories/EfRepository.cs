using Domain.Seedwork;

namespace PensamientoAlternativo.Persistance.Repositories
{
    public class EfRepository<T> : Repository<T> where T : Entity
    {
        public EfRepository(AppDbContext context) : base(context) { }
    }
}
