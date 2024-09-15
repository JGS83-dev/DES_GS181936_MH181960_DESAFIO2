using System.ComponentModel.DataAnnotations;

namespace UsuarioAPI.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public List<RolPermiso> RolPermisos { get; } = [];
    }
}
