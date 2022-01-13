﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SISCOSMAC.DAL.DbContextSql;

namespace SISCOSMAC.DAL.Migrations
{
    [DbContext(typeof(ContextSql))]
    [Migration("20220104164502_addHashSal")]
    partial class addHashSal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SISCOSMAC.DAL.Models.Departamento", b =>
                {
                    b.Property<int>("DepartamentoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NombreDepartamento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("OrdenTrabajo")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("DepartamentoId");

                    b.ToTable("Departamento");
                });

            modelBuilder.Entity("SISCOSMAC.DAL.Models.SolicitudMantenimientoCorrectivo", b =>
                {
                    b.Property<int>("SolicitudId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AreaSolicitante")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DepartamentoDirigido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DescripcionServicios")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("FechaElaboracion")
                        .HasColumnType("datetime");

                    b.Property<int?>("Folio")
                        .HasColumnType("int");

                    b.Property<string>("NombreSolicitante")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("SolicitudId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("SolicitudMantenimientoCorrectivo");
                });

            modelBuilder.Entity("SISCOSMAC.DAL.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AMaterno")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("APaterno")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DepartamentoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(32)")
                        .HasMaxLength(32);

                    b.Property<byte[]>("PasswordSal")
                        .IsRequired()
                        .HasColumnType("varbinary(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UsuarioLogin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UsuarioId");

                    b.HasIndex("DepartamentoId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SISCOSMAC.DAL.Models.SolicitudMantenimientoCorrectivo", b =>
                {
                    b.HasOne("SISCOSMAC.DAL.Models.Usuario", "usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SISCOSMAC.DAL.Models.Usuario", b =>
                {
                    b.HasOne("SISCOSMAC.DAL.Models.Departamento", "departamento")
                        .WithMany()
                        .HasForeignKey("DepartamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
