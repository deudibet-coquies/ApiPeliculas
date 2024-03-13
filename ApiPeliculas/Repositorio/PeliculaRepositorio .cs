using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public PeliculaRepositorio(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.fechaCreacion = DateTime.Now;
            _applicationDbContext.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _applicationDbContext.Peliculas.Remove(pelicula);
            return Guardar();
        }

       
        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.fechaCreacion = DateTime.Now;
            _applicationDbContext.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool existe = _applicationDbContext.Peliculas.Any(c => c.nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return existe;
        }

        public bool ExistePelicula(int id)
        {
            return _applicationDbContext.Peliculas.Any(c => c.Id == id);
            
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _applicationDbContext.Peliculas.OrderBy(c => c.nombre).ToList();
        }

        public Pelicula GetPeliculas(int peliculaId)
        {
            return _applicationDbContext.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculasCategorias(int catId)
        {
           return _applicationDbContext.Peliculas.Include(ca => ca.categoria).Where(ca => ca.categoriaId == catId).ToList();
        }

        public ICollection<Pelicula> GetPeliculasNombre(string nombre)
        {
            IQueryable<Pelicula> querey = _applicationDbContext.Peliculas;

            if (!string.IsNullOrEmpty(nombre))
            {
                querey = querey.Where(e => e.nombre.Contains(nombre) || e.descripcion.Contains(nombre));
            }
            return querey.ToList();
        }

        public bool Guardar()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }
    }
}
