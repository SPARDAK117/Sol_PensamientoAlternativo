using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Constantes
{
    public static class SupportedViews
    {
        public const string Home = "Home";
        public const string About = "About";
        public const string Contact = "Contact";

        public static readonly Dictionary<string, string> DefaultSections = new(StringComparer.OrdinalIgnoreCase)
        {
            { Home, "Home" },
            { About, "Sobre Nosotros" },
            { Contact, "Contáctanos" }
        };
    }

}
