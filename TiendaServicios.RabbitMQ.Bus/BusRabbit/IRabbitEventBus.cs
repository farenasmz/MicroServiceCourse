using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.Comandos;
using TiendaServicios.RabbitMQ.Bus.Eventos;

namespace TiendaServicios.RabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task EnviarComando<T>(T command) where T : Comando;
        void Publis<T>(T @evento) where T : Evento;
        void Subscribe<T, TH>() where T : Evento
                                where TH : IEventoManejador<T>;
    }
}
