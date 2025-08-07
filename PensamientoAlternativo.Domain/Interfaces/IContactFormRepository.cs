using Domain.Seedwork;
using PensamientoAlternativo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IContactFormRepository : IRepository<ContactForm>
    {
        Task DeleteAsync(int id);
    }
}
