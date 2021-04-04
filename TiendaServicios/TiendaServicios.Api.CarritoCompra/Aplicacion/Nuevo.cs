using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto Contexto;

            public Manejador(CarritoContexto contexto)
            {
                Contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                int value;
                CarritoSesion carritoSesion = new CarritoSesion()
                {
                    FechaCompra = request.FechaCreacionSesion
                };

                Contexto.CarritoSesion.Add(carritoSesion);
                value = await Contexto.SaveChangesAsync();
                
                if (value == 0)
                {
                    throw new Exception("Error en la inserción");
                }

                foreach (string item in request.ProductoLista)
                {
                    Contexto.CarritoSesionDetalle.Add(new CarritoSesionDetalle
                    {
                        CarritoSesionId = carritoSesion.CarritoSesionId,
                        FechaCompra = DateTime.Now,
                        ProductoSeleccionado = item
                    });
                }

                value = await Contexto.SaveChangesAsync();

                if (value > 0)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new Exception("Error al insertar el detalle de carrito de compra.");
                }
            }
        }
    }
}
