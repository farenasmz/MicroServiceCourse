﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TiendaServicio.Api.Libro.Persistencia;

namespace TiendaServicio.Api.Libro.Migrations
{
    [DbContext(typeof(ContextoLibreria))]
    [Migration("20210504214332_ApiLibroInitial")]
    partial class ApiLibroInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TiendaServicio.Api.Libro.Modelo.LibreriaMaterial", b =>
                {
                    b.Property<Guid?>("LibreriaMaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AutorLibro")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("FechaPublicacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LibreriaMaterialId");

                    b.ToTable("LibreriaMaterial");
                });
#pragma warning restore 612, 618
        }
    }
}
