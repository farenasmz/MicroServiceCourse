using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicio.Api.Libro.Modelo;
using TiendaServicio.Api.Libro.Persistencia;

namespace TiendaServicio.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibroMaterialDto>
        {
            public Guid LibroGuid { get; set; }
        }

        public class Manejador : IRequestHandler<LibroUnico, LibroMaterialDto>
        {
            private readonly ContextoLibreria Context;
            private readonly IMapper Mapper;

            public Manejador(ContextoLibreria context, IMapper mapper)
            {
                Context = context;
                Mapper = mapper;
            }

            public async Task<LibroMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                LibreriaMaterial result = await Context.LibreriaMaterial.FirstOrDefaultAsync(a => a.LibreriaMaterialId == request.LibroGuid);

                if (result == null)
                {
                    throw new Exception("No existe");
                }

                LibroMaterialDto dto = Mapper.Map<LibreriaMaterial, LibroMaterialDto>(result);
                return dto;
            }
        }
    }
}
