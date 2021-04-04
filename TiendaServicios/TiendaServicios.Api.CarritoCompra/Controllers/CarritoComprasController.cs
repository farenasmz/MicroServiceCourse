using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Aplicacion;

namespace TiendaServicios.Api.CarritoCompra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoComprasController : ControllerBase
    {
        private readonly IMediator Mediator;

        public CarritoComprasController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoDto>> GetCarrito(int id)
        {
            return await Mediator.Send(new Consulta.Ejecuta { CarritoSesionId = id });
        }
    }
}