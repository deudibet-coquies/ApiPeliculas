using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dto;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace ApiPeliculas.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly IUsuarioRepositorio _usRepo;
        protected RespuestaApi _respuestaApi;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            this._respuestaApi = new();
            _mapper = mapper;
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{usuarioId}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetUsuario(string usuarioId)
        {
            var itenUsuario = _usRepo.GetUsuario(usuarioId);

            if (itenUsuario == null)
            {
                return NotFound();
            }
            var itenUsuarioDto = _mapper.Map<UsuarioDto>(itenUsuario);
            return Ok(itenUsuarioDto);
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool validarUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.nombreUsuario);
            if (!validarUsuarioUnico)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.Result = false;
                _respuestaApi.ErrorMessages.Add("el susuario ya existe en el sistema");
                return BadRequest(_respuestaApi);
            }        

            var usuario = await _usRepo.Registro(usuarioRegistroDto);
            if (usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.Result = false;
                _respuestaApi.ErrorMessages.Add("erroer en el registro");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return Ok(_respuestaApi);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _usRepo.Login(usuarioLoginDto);
            if (respuestaLogin.usuario == null || string.IsNullOrEmpty(respuestaLogin.token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.Result = false;
                _respuestaApi.ErrorMessages.Add("el nombreUsuario o passwod son incorrectos");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLogin;
            return Ok(_respuestaApi);
        }





    }
}
