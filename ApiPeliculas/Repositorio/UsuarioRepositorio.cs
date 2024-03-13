using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dto;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XAct;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext applicationDbContext,
            IConfiguration config,
            RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _applicationDbContext.AppUsuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _applicationDbContext.AppUsuario.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioDb = _applicationDbContext.AppUsuario.FirstOrDefault(u => u.UserName == usuario);
            if (usuarioDb == null)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }


        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
          //  var passwordEncriptada = obtenermd5(usuarioRegistroDto.password);

            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.nombreUsuario,
                Email = usuarioRegistroDto.nombreUsuario,
                NormalizedEmail = usuarioRegistroDto.nombreUsuario.ToUpper(),
                nombre = usuarioRegistroDto.nombre,
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.password);
            if (result.Succeeded)
            {
                // pro primera vez y creacion de roles
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                await _userManager.AddToRoleAsync(usuario, "registrado");
                var usuarioRetornado = _applicationDbContext.AppUsuario.FirstOrDefault( u => u.UserName == usuarioRegistroDto.nombreUsuario);
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    nombre = usuarioRetornado.nombre

                //};
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }

            //_applicationDbContext.Usuarios.Add(usuario);
            //await _applicationDbContext.SaveChangesAsync();
            //usuario.password = passwordEncriptada;
            //return usuario;

            return new UsuarioDatosDto();
        }






        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
           // var passwordEncriptada = obtenermd5(usuarioLoginDto.password);

            var usuarios = _applicationDbContext.AppUsuario.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.nombreUsuario.ToLower());
           
            bool isValidate = await _userManager.CheckPasswordAsync(usuarios, usuarioLoginDto.password);
            
            // se valida si el usuario existe
            if (usuarios == null || isValidate == false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    token = "",
                    usuario = null
                };
            }

            // usuario encontrado
            var roles = await _userManager.GetRolesAsync(usuarios);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, usuarios.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescription);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                token = manejadorToken.WriteToken(token),
                usuario = _mapper.Map<UsuarioDatosDto>(usuarios)
            };

            return usuarioLoginRespuestaDto;
        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

       
    }
}
