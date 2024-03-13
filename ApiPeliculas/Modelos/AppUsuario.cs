using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    public class AppUsuario : IdentityUser
    {

        // se pueden agragar campos personalizados 

        public string nombre { get; set; }

    }
}
