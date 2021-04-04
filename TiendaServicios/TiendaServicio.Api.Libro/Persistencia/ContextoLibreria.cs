using Microsoft.EntityFrameworkCore;
using TiendaServicio.Api.Libro.Modelo;

namespace TiendaServicio.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria()
        {

        }

        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options)
        {
        }

        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
    }
}
