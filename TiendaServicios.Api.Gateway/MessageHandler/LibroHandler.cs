using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;

        public LibroHandler(ILogger<LibroHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation)
        {
            Stopwatch tiempo = Stopwatch.StartNew();
            HttpResponseMessage response;
            string contenido;
            JsonSerializerOptions options;
            LibroModeloRemote resultado;

            _logger.LogInformation("Inicia Request");
            response = await base.SendAsync(request, cancellation);

            if (response.IsSuccessStatusCode)
            {
                contenido = await response.Content.ReadAsStringAsync();
                options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);
            }

            tiempo.Stop();
            _logger.LogInformation($"Proceso demoró: {tiempo.ElapsedMilliseconds} ms.");
            return response;
        }
    }
}
