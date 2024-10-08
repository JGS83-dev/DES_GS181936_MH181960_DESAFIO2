﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UsuarioAPI.Models;

#nullable disable

namespace UsuarioAPI.Migrations
{
    [DbContext(typeof(UsuarioContext))]
    [Migration("20240914223243_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UsuarioAPI.Models.Permiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permisos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Crear Usuarios",
                            Nombre = "Crear Usuarios"
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "Leer Usuarios",
                            Nombre = "Leer Usuarios"
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "Actualizar Usuarios",
                            Nombre = "Actualizar Usuarios"
                        },
                        new
                        {
                            Id = 4,
                            Descripcion = "Eliminar Usuarios",
                            Nombre = "Eliminar Usuarios"
                        });
                });

            modelBuilder.Entity("UsuarioAPI.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Rol: Administrador",
                            Nombre = "Administrador"
                        });
                });

            modelBuilder.Entity("UsuarioAPI.Models.RolPermiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PermisoId")
                        .HasColumnType("int");

                    b.Property<int?>("RolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermisoId");

                    b.HasIndex("RolId");

                    b.ToTable("RolPermisos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PermisoId = 1,
                            RolId = 1
                        },
                        new
                        {
                            Id = 2,
                            PermisoId = 2,
                            RolId = 1
                        },
                        new
                        {
                            Id = 3,
                            PermisoId = 3,
                            RolId = 1
                        },
                        new
                        {
                            Id = 4,
                            PermisoId = 4,
                            RolId = 1
                        });
                });

            modelBuilder.Entity("UsuarioAPI.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "sysadmin@udb.edu.sv",
                            Nombre = "SysAdmin",
                            Password = "$2a$11$z1FKSFFada5GQuZ7tUx6e.oPwgRY1BYQhoJmLFwwzpIqOvebX6qC6",
                            RolId = 1
                        });
                });

            modelBuilder.Entity("UsuarioAPI.Models.RolPermiso", b =>
                {
                    b.HasOne("UsuarioAPI.Models.Permiso", "Permiso")
                        .WithMany("RolPermisos")
                        .HasForeignKey("PermisoId");

                    b.HasOne("UsuarioAPI.Models.Rol", "Rol")
                        .WithMany("RolPermisos")
                        .HasForeignKey("RolId");

                    b.Navigation("Permiso");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("UsuarioAPI.Models.Usuario", b =>
                {
                    b.HasOne("UsuarioAPI.Models.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolId");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("UsuarioAPI.Models.Permiso", b =>
                {
                    b.Navigation("RolPermisos");
                });

            modelBuilder.Entity("UsuarioAPI.Models.Rol", b =>
                {
                    b.Navigation("RolPermisos");
                });
#pragma warning restore 612, 618
        }
    }
}
