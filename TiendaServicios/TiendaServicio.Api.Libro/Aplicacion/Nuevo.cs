using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicio.Api.Libro.Modelo;
using TiendaServicio.Api.Libro.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicio.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(r => r.Titulo).NotEmpty();
                RuleFor(r => r.FechaPublicacion).NotEmpty();
                RuleFor(r => r.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria Context;
            private readonly IRabbitEventBus EventBus;

            public Manejador(ContextoLibreria context)
            {
                Context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                int result;
                LibreriaMaterial autorLibro = new()
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro,
                };

                await Context.AddAsync(autorLibro);
                result = await Context.SaveChangesAsync();

                if (result > 0)
                {
                    EventBus.Publis(new EmailEventoQueue("farenas1@misena.edu.co", request.Titulo, "Contenido de ejemplo"));
                    return Unit.Value;
                }

                throw new Exception("Ha ocurrido un error");
            }
        }
    }
}
