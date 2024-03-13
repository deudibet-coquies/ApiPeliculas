using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public CategoriaRepositorio(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.fechaCreacion = DateTime.Now;
            _applicationDbContext.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCegoria(Categoria categoria)
        {
            _applicationDbContext.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.fechaCreacion = DateTime.Now;
            _applicationDbContext.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool existe = _applicationDbContext.Categorias.Any(c => c.nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return existe;
        }

        public bool ExisteCategoria(int id)
        {
            return _applicationDbContext.Categorias.Any(c => c.Id == id);
            
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _applicationDbContext.Categorias.OrderBy(c => c.nombre).ToList();
        }

        public Categoria GetCategorias(int categoriaId)
        {
            return _applicationDbContext.Categorias.FirstOrDefault(c => c.Id == categoriaId);
        }

        public bool Guardar()
        {
            return _applicationDbContext.SaveChanges() >= 0 ? true : false;
        }
    }
}
