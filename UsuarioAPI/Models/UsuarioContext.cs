using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace UsuarioAPI.Models
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            

            base.OnModelCreating(modelBuilder);

            // Siembra de datos para la tabla Permisos

            modelBuilder.Entity<Permiso>().HasData(
                new Permiso
                {
                    Id = 1,
                    Nombre = "Crear Usuarios",
                    Descripcion = "Crear Usuarios"
                },
                new Permiso
                {
                    Id = 2,
                    Nombre = "Leer Usuarios",
                    Descripcion = "Leer Usuarios"
                },
                new Permiso
                {
                    Id = 3,
                    Nombre = "Actualizar Usuarios",
                    Descripcion = "Actualizar Usuarios"
                },
                new Permiso
                {
                    Id = 4,
                    Nombre = "Eliminar Usuarios",
                    Descripcion = "Eliminar Usuarios"
                }
            );

            // Siembra de datos para la tabla Roles

            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Descripcion = "Rol: Administrador"
                }                
            );

            // Siembra de datos para la tabla RolPermisos

            modelBuilder.Entity<RolPermiso>().HasData(
                new RolPermiso { Id = 1, PermisoId = 1, RolId = 1 },
                new RolPermiso { Id = 2, PermisoId = 2, RolId = 1 },
                new RolPermiso { Id = 3, PermisoId = 3, RolId = 1 },
                new RolPermiso { Id = 4, PermisoId = 4, RolId = 1 }
            );

            // Siembra de datos para la tabla Usuarios

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "SysAdmin",
                    Email = "sysadmin@udb.edu.sv",
                    Password = BCrypt.Net.BCrypt.HashPassword("aguanteRiver"),
                    RolId = 1,
                }
            );
        }
    }
}
