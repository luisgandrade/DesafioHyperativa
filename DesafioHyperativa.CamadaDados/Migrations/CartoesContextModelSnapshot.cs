﻿// <auto-generated />
using DesafioHyperativa.CamadaDados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DesafioHyperativa.CamadaDados.Migrations
{
    [DbContext(typeof(CartoesContext))]
    partial class CartoesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.7");

            modelBuilder.Entity("DesafioHyperativa.CamadaDados.Entidades.Cartao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroCartao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cartoes");
                });
#pragma warning restore 612, 618
        }
    }
}
