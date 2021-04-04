using AutoMapper;
using TiendaServicio.Api.Libro.Aplicacion;
using TiendaServicio.Api.Libro.Modelo;

namespace TiendaServicios.Apil.Libro.Tests
{
    public class MappingTest : Profile
    {
        public MappingTest()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
