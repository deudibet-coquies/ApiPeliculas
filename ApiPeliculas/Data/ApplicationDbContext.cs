using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{

   // public class ApplicationDbContext : DbContext esto si no uso el identity
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pelicula> Peliculas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
