using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>> { }

        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly ContextoAutor Contexto;
            private readonly IMapper Mapper;

            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                Contexto = contexto;
                Mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                List<AutorLibro> result = await Contexto.AutorLibro.ToListAsync();
                List<AutorDto> dto = Mapper.Map<List<AutorLibro>, List<AutorDto>>(result);
                return dto;
            }
        }
    }
}
