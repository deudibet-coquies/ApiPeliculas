using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> GetCategorias();

        Categoria GetCategorias(int categoriaId);

        bool ExisteCategoria(string nombre);

        bool ExisteCategoria(int id);    

        bool CrearCategoria(Categoria categoria);

        bool ActualizarCategoria(Categoria categoria);

        bool BorrarCegoria(Categoria categoria);

        bool Guardar();


    }
}
