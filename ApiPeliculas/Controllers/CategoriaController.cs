using ApiPeliculas.Migrations;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dto;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
   
    //[Route("api/[controller]")]
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {            
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        // [ResponseCache(Duration = 10)]
        [ResponseCache(CacheProfileName = "PorDefecto30Segundos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();           
            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }

        [AllowAnonymous]
        [HttpGet("{categoriaId:int}",Name = "GetCategoria")]
        //[ResponseCache(Duration = 30)]
        [ResponseCache(CacheProfileName = "PorDefecto30Segundos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itenCategoria = _ctRepo.GetCategorias(categoriaId);

            if (itenCategoria == null)
            {
                return NotFound();
            }
            var itenCategoriaDto = _mapper.Map<CategoriaDto>(itenCategoria);
            return Ok(itenCategoriaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]        
        [ProducesResponseType(200 ,Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExisteCategoria(crearCategoriaDto.nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe en el sistema.");
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"algo salio mal guardando el registro {categoria.nombre}.");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetCategoria", new {categoriaId = categoria.Id}, categoria);

        }


        [Authorize(Roles = "admin")]
        [HttpPatch("{categoriaId:int}", Name = "ActualizarPathCategoria")]
        [ProducesResponseType(200, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPathCategoria(int categoriaId, [FromBody] CategoriaDto CategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (CategoriaDto == null || categoriaId != CategoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(CategoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"algo salio mal actualizando el registro {categoria.nombre}.");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategorias")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult BorrarCategorias(int categoriaId)
        {

            if (!_ctRepo.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            var categoria = _ctRepo.GetCategorias(categoriaId);
            if (!_ctRepo.BorrarCegoria(categoria))
            {
                ModelState.AddModelError("", $"algo salio mal borrando el registro {categoria.nombre}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }




    }
}
