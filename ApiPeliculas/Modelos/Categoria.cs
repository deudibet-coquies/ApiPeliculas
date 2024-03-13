using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string nombre { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
