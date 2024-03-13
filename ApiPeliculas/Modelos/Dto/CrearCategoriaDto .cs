using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dto
{
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio para crear la categoria")]
        [MaxLength(100, ErrorMessage = "Supera el numero maximo permitido de caracteres.")]
        public string nombre { get; set; }      
    }
}
