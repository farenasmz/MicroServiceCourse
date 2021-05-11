using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Comandos;
using TiendaServicios.RabbitMQ.Bus.Eventos;

namespace TiendaServicios.RabbitMQ.Bus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator Mediator;
        private readonly Dictionary<string, List<Type>> Manejadores;
        private readonly List<Type> EventoTipos;
        private readonly IServiceScopeFactory ServiceFactory;

        public RabbitEventBus(IServiceScopeFactory serviceFactory, IMediator mediator, Dictionary<string, List<Type>> manejadores, List<Type> eventoTipos)
        {
            Mediator = mediator;
            Manejadores = manejadores;
            EventoTipos = eventoTipos;
            ServiceFactory = serviceFactory;
        }

        public RabbitEventBus(IMediator mediator)
        {
            Mediator = mediator;
            Manejadores = new Dictionary<string, List<Type>>();
            EventoTipos = new List<Type>();
        }

        public Task EnviarComando<T>(T command) where T : Comando
        {
            return Mediator.Send(command);
        }

        public void Publis<T>(T evento) where T : Evento
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "rabbit-vaxi-web"
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            string eventName = evento.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            string message = JsonConvert.SerializeObject(evento);
            byte[] body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", eventName, null, body);
        }

        public void Subscribe<T, TH>()
            where T : Evento
            where TH : IEventoManejador<T>
        {
            string eventoNombre = typeof(T).Name;
            Type manejadorEventoTipo = typeof(TH);

            if (!EventoTipos.Contains(typeof(T)))
            {
                EventoTipos.Add(typeof(T));
            }

            if (!Manejadores.ContainsKey(eventoNombre))
            {
                Manejadores.Add(eventoNombre, new List<Type>());
            }

            if (Manejadores[eventoNombre].Any(x => x.GetType() == manejadorEventoTipo))
            {
                throw new ArgumentException($"El manejador {manejadorEventoTipo.Name} fue registrado anteriormente por {eventoNombre}");
            }

            Manejadores[eventoNombre].Add(manejadorEventoTipo);
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "rabbit-vaxi-web",
                DispatchConsumersAsync = true
            };

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(eventoNombre, false, false, false, null);
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;
            channel.BasicConsume(eventoNombre, true, consumer);

        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            string nombreEvento = e.RoutingKey;
            string message = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                if (Manejadores.ContainsKey(nombreEvento))
                {
                    using IServiceScope scope = ServiceFactory.CreateScope();

                    List<Type> subscriptions = Manejadores[nombreEvento];

                    foreach (Type item in subscriptions)
                    {
                        object manejador = scope.ServiceProvider.GetService(item); //Activator.CreateInstance(item);

                        if (manejador == null) continue;

                        Type tipoEvento = EventoTipos.SingleOrDefault(x => x.Name == nombreEvento);
                        object eventoDS = JsonConvert.DeserializeObject(message, tipoEvento);
                        Type concretoTipo = typeof(IEventoManejador<>).MakeGenericType(tipoEvento);
                        await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
