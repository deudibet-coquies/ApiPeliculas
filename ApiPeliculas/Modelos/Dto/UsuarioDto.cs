using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dto
{
    public class UsuarioDto
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public string role { get; set; }

    }
}
