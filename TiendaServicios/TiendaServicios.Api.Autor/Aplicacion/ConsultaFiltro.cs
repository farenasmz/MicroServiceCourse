using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class ConsultaFiltro
    {
        public class AutorUnico : IRequest<AutorDto>
        {
            public string AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<AutorUnico, AutorDto>
        {
            private readonly ContextoAutor Context;
            private readonly IMapper Mapper;

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                Context = context;
                Mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
            {
                AutorLibro result = await Context.AutorLibro.FirstOrDefaultAsync(a => a.AutorLibroGuid == request.AutorGuid);
                AutorDto dto = Mapper.Map<AutorLibro, AutorDto>(result);
                return dto;
            }
        }
    }
}
