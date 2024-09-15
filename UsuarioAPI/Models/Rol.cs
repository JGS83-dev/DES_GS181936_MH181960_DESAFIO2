using System.ComponentModel.DataAnnotations;

namespace UsuarioAPI.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;
        
        public string? Descripcion { get; set; }

        public List<RolPermiso> RolPermisos { get; } = [];
    }
}
