using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TiendaServicio.Api.Libro.Aplicacion;
using TiendaServicio.Api.Libro.Persistencia;

namespace TiendaServicio.Api.Libro
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
            services.AddControllers();
            services.AddDbContext<ContextoLibreria>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("ConexionDatabase"));
            });
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Ejecuta>());
            services.AddMediatR(typeof(Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Ejecuta));
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
        }
    }
}
