using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPeliculas();

        Pelicula GetPeliculas(int PeliculaId);

        bool ExistePelicula(string nombre);

        bool ExistePelicula(int id);    

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);


        ICollection<Pelicula> GetPeliculasCategorias(int catId);
        ICollection<Pelicula> GetPeliculasNombre(string nombre);

        bool Guardar();


    }
}
