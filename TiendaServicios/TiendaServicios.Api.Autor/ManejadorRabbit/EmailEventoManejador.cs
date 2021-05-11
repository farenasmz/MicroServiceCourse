using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> Logger;

        public EmailEventoManejador()
        {

        }

        public EmailEventoManejador(ILogger<EmailEventoManejador> logger)
        {
            Logger = logger;
        }

        public Task Handle(EmailEventoQueue @event)
        {
            Logger.LogInformation($"Este es el valor que consumo desde Rabbitmq { @event.Titulo}");
            return Task.CompletedTask;
        }
    }
}
