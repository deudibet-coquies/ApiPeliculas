using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Usuario
    {


        [Key]
        public int Id { get; set; }

        public string nombreUsuario { get; set; }

        public string nombre { get; set; }

        public string password { get; set; }

        public string role { get; set; }

    }
}
