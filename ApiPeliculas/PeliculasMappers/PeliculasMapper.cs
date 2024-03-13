using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dto;
using AutoMapper;

namespace ApiPeliculas.PeliculasMappers
{
    public class PeliculasMapper : Profile
    {

        public PeliculasMapper()
        {          
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
        }

    }
}
