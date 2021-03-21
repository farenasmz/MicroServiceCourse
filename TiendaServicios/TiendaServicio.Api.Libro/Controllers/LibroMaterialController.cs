using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServicio.Api.Libro.Aplicacion;

namespace TiendaServicio.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator Mediator;

        public LibroMaterialController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [Route(""), HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            return await Mediator.Send(new Consulta.Ejecuta());
        }

        [Route("{id}"), HttpGet]
        public async Task<ActionResult<LibroMaterialDto>> GetLibro(Guid id)
        {
            return await Mediator.Send(new ConsultaFiltro.LibroUnico { LibroGuid = id });
        }
    }
}
