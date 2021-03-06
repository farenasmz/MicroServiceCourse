using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.ManejadorRabbit;
using TiendaServicios.Api.Autor.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;
using TiendaServicios.RabbitMQ.Bus.Implement;

namespace TiendaServicios.Api.Autor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRabbitEventBus, RabbitEventBus>();

            services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();

            services.AddControllers().AddFluentValidation(cfg =>
                cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>()
            );

            services.AddDbContext<ContextoAutor>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDatabase"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Configuration["Parameters:SwaggerTitle"],
                    Version = "v1",
                    Description = Configuration["Parameters:SwaggerDescriptionMessage"],
                    Contact = new OpenApiContact()
                    {
                        Name = Configuration["Parameters:SwaggerName"],
                        Email = Configuration["Parameters:SwaggerContact"]
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["Parameters:SwaggerTitle"]);
            });

            IRabbitEventBus eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailEventoQueue, EmailEventoManejador>();
        }
    }
}
