using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : IlibroService
    {
        private readonly IHttpClientFactory HttpClient;
        private readonly ILogger<LibrosService> Logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            HttpResponseMessage response;

            try
            {
                using (HttpClient cliente = HttpClient.CreateClient("Libros"))
                {
                    response = await cliente.GetAsync($"api/LibroMaterial/{LibroId}");

                    if (response.IsSuccessStatusCode)
                    {
                        string contenido = await response.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        LibroRemote resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                        return (true, resultado, null);
                    }
                };

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
