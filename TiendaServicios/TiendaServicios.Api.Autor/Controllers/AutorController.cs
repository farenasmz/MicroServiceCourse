using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator Mediator;

        public AutorController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [Route(""), HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await Mediator.Send(new Consulta.ListaAutor());
        }

        [Route("{id}"), HttpGet]
        public async Task<ActionResult<AutorDto>> GetAutor(string id)
        {
            return await Mediator.Send(new ConsultaFiltro.AutorUnico { AutorGuid = id });
        }
    }
}
