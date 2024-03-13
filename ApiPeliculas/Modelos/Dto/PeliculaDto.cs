using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dto
{
    public class PeliculaDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { get; set; }
        public byte[] rutaImagen { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "La duracion es obligatoria")]
        public int duracion { get; set; }
        public enum tipoClasificacion { Siete, Trece, Dieciseis, Veinte }
        public tipoClasificacion clasificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int categoriaId { get; set; }


    }
}
