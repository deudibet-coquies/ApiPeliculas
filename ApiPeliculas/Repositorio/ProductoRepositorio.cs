using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        public bool AgregarProducto(Producto producto)
        {
            throw new NotImplementedException();
        }

        public Producto ObtenerProductoPorId(int productoId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Producto> ObtenerTodosLosProductos()
        {
            throw new NotImplementedException();
        }
    }
}
