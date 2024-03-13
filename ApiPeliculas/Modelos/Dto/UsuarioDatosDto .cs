using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dto
{
    public class UsuarioDatosDto
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string nombre { get; set; }

    }
}
