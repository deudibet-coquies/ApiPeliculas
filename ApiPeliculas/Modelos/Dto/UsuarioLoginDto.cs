using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dto
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El nombreUsuario es obligatorio")]
        public string nombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]      
        public string password { get; set; }
      
    }
}
