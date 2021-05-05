using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {

        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto Contexto;
            private readonly IlibroService LibroService;

            public Manejador(CarritoContexto contexto, IlibroService libroService)
            {
                Contexto = contexto;
                LibroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                CarritoSesion carritoSesion = await Contexto.CarritoSesion.FirstOrDefaultAsync(c => c.CarritoSesionId == request.CarritoSesionId);
                List<CarritoSesionDetalle> carritoSesionDetalle = await Contexto.CarritoSesionDetalle.Where(c => c.CarritoSesionId == request.CarritoSesionId).ToListAsync();
                List<CarritoDetalleDto> listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (CarritoSesionDetalle libro in carritoSesionDetalle)
                {
                    (bool resultado, RemoteModel.LibroRemote Libro, string ErrorMessage) response = await LibroService.GetLibro(new Guid(libro.ProductoSeleccionado));

                    if (response.resultado)
                    {
                        LibroRemote objectoLibro = response.Libro;
                        CarritoDetalleDto carritoDetalle = new CarritoDetalleDto()
                        {
                            TituloLibro = objectoLibro.Titulo,                            
                            FechaPublicacion = objectoLibro.FechaPublicacion,
                            LibroId = objectoLibro.LibreriaMaterialId
                        };
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                return new CarritoDto
                {
                    CarritoId = carritoSesion?.CarritoSesionId,
                    ListaProductos = listaCarritoDto,
                    FechaCreacionSesion = carritoSesion?.FechaCompra
                };
            }
        }
    }
}
