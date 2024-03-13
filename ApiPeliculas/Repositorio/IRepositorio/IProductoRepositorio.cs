using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IProductoRepositorio
    {
        ICollection<Producto> ObtenerTodosLosProductos();

        Producto ObtenerProductoPorId(int productoId);

        bool AgregarProducto(Producto producto);
    }
}
