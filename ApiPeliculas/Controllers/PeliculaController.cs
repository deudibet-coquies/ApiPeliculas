using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dto;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        public PeliculaController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itenPelicula = _pelRepo.GetPeliculas(peliculaId);

            if (itenPelicula == null)
            {
                return NotFound();
            }
            var itenPeliculaDto = _mapper.Map<PeliculaDto>(itenPelicula);
            return Ok(itenPeliculaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPelicula([FromBody] PeliculaDto crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelRepo.ExistePelicula(crearPeliculaDto.nombre))
            {
                ModelState.AddModelError("", "La Pelicula ya existe en el sistema.");
                return StatusCode(404,ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"algo salio mal guardando el registro {pelicula.nombre}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);

        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPathPelicula")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPathPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }          

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"algo salio mal actualizando el registro {pelicula.nombre}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult BorrarPelicula(int peliculaId)
        {

            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelRepo.GetPeliculas(peliculaId);
            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"algo salio mal borrando el registro {pelicula.nombre}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }


        [AllowAnonymous]
        [HttpGet("GetPeliculasCategorias/{categoriaId:int}")]
        public IActionResult GetPeliculasCategorias(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasCategorias(categoriaId);
            if (listaPeliculas == null)
            {
                return NotFound();
            }
            var itemPelicula = new List<PeliculaDto>();
            foreach (var iten in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(iten));
            }
            return Ok(itemPelicula);
        }


        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
           
            try
            {
                var resultado = _pelRepo.GetPeliculasNombre(nombre.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
               
            }
        }



    }
}
