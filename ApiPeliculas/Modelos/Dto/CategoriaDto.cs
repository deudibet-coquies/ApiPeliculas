using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dto
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Supera el numero maximo permitido de caracteres.")]
        public string nombre { get; set; }       

    }
}
