using AutoMapper;
using GenFu;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TiendaServicio.Api.Libro.Aplicacion;
using TiendaServicio.Api.Libro.Modelo;
using TiendaServicio.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Apil.Libro.Tests
{
    public class LibrosServiceTest
    {
        private Mock<ContextoLibreria> CrearContext()
        {
            IQueryable<LibreriaMaterial> dataPrueba = ObtenerDataPrueba().AsQueryable();
            Mock<DbSet<LibreriaMaterial>> dbSet = new Mock<DbSet<LibreriaMaterial>>();
            Mock<ContextoLibreria> contexto;
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());
            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken())).Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            List<LibreriaMaterial> lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;
            return lista;
        }

        [Fact]
        public async void GetLibroById()
        {
            Mock<ContextoLibreria> mockContexto = CrearContext();
            MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            IMapper mapper = mapConfig.CreateMapper();
            ConsultaFiltro.Manejador manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);
            ConsultaFiltro.LibroUnico request = new ConsultaFiltro.LibroUnico()
            {
                LibroGuid = Guid.Empty
            };

            LibroMaterialDto result = await manejador.Handle(request, new System.Threading.CancellationToken());
            Assert.NotNull(result);
            Assert.True(result.LibreriaMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros()
        {
            Mock<ContextoLibreria> mockContexto = CrearContext();
            MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile(new MappingTest());
              });
            IMapper mapper = mapConfig.CreateMapper();
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);
            Consulta.Ejecuta request = new Consulta.Ejecuta();
            List<LibroMaterialDto> lista = await manejador.Handle(request, new System.Threading.CancellationToken());
            Assert.True(lista.Any());
        }

        [Fact]
        public async void GuardarLibro()
        {
            DbContextOptions<ContextoLibreria> options = new DbContextOptionsBuilder<ContextoLibreria>().UseInMemoryDatabase(databaseName: "BaseDatosLibro").Options;

            ContextoLibreria contexto = new ContextoLibreria(options);
            MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            IMapper mapper = mapConfig.CreateMapper();
            Nuevo.Ejecuta request = new Nuevo.Ejecuta()
            {
                Titulo = "Libro",
                AutorLibro = Guid.Empty,
                FechaPublicacion = DateTime.Now
            };

            Nuevo.Manejador manejador = new Nuevo.Manejador(contexto);
            Unit result = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(result != null);
        }
    }
}
