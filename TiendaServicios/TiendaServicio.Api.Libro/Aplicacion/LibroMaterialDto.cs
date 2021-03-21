using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicio.Api.Libro.Aplicacion
{
    public class LibroMaterialDto
    {
        public string Titulo1 { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid AutorLibro { get; set; }
    }
}
