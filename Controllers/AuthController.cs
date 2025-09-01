using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SistemaReciclaje.Data;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ReciclajeDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ReciclajeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var usuario = await _context.UsuariosSistema
                .FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario && u.Activo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return BadRequest("Credenciales inválidas");
            }

            // Actualizar último acceso
            usuario.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            var token = GenerarToken(usuario);

            return Ok(new LoginResponse
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Rol = usuario.Rol,
                Token = token
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioSistema>> Register(RegistroUsuarioRequest request)
        {
            if (await _context.UsuariosSistema.AnyAsync(u => u.NombreUsuario == request.NombreUsuario))
            {
                return BadRequest("El nombre de usuario ya existe");
            }

            var usuario = new UsuarioSistema
            {
                NombreUsuario = request.NombreUsuario,
                NombreCompleto = request.NombreCompleto,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Rol = request.Rol
            };

            _context.UsuariosSistema.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<ActionResult<UsuarioSistema>> ObtenerPerfil()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var usuario = await _context.UsuariosSistema.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPut("cambiar-password")]
        [Authorize]
        public async Task<ActionResult> CambiarPassword(CambiarPasswordRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var usuario = await _context.UsuariosSistema.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(request.PasswordActual, usuario.PasswordHash))
            {
                return BadRequest("Contraseña actual incorrecta");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordNueva);
            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada exitosamente");
        }

        [HttpGet("usuarios")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<UsuarioSistema>>> ListarUsuarios()
        {
            var usuarios = await _context.UsuariosSistema.ToListAsync();
            return Ok(usuarios);
        }

        private string GenerarToken(UsuarioSistema usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "clave-super-secreta-para-desarrollo-2024");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}