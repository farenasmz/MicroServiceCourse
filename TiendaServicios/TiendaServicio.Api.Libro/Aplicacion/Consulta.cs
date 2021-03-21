using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicio.Api.Libro.Modelo;
using TiendaServicio.Api.Libro.Persistencia;

namespace TiendaServicio.Api.Libro.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<LibroMaterialDto>> { }

        public class Manejador : IRequestHandler<Ejecuta, List<LibroMaterialDto>>
        {
            private readonly ContextoLibreria Context;
            private readonly IMapper Mapper;

            public Manejador(ContextoLibreria context, IMapper mapper)
            {
                Context = context;
                Mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                List<LibreriaMaterial> result = await Context.LibreriaMaterial.ToListAsync();
                List<LibroMaterialDto> dto = Mapper.Map<List<LibreriaMaterial>, List<LibroMaterialDto>>(result);
                return dto;
            }
        }
    }
}
