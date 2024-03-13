using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {

        [Key]
        public int Id { get; set; }
      
        public string nombre { get; set; }

        public byte[] rutaImagen { get; set; }

        public string descripcion { get; set; }

        public int duracion { get; set; }

        public enum tipoClasificacion { Siete, Trece, Dieciseis, Veinte }

        public  tipoClasificacion clasificacion { get; set; }

        public  DateTime fechaCreacion{ get; set; }

        [ForeignKey("categoriaId")]
        public int categoriaId { get; set; }

        public Categoria categoria { get; set;}

    }
}
