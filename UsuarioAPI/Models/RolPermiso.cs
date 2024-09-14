using System.ComponentModel.DataAnnotations;

namespace UsuarioAPI.Models
{
    public class RolPermiso
    {
        [Key]
        public int Id { get; set; }

        public int? RolId { get; set; }

        public Rol Rol { get; set; }

        public int? PermisoId { get; set; }

        public Permiso Permiso { get; set; }
    }
}
