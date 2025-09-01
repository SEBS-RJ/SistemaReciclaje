using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class UsuarioSistema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore] // No enviar la contraseña en las respuestas
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public TipoRol Rol { get; set; } = TipoRol.Cajero;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? UltimoAcceso { get; set; }
    }

    public enum TipoRol
    {
        Administrador = 1,
        Cajero = 2,
        Supervisor = 3
    }

    // DTO para login
    public class LoginRequest
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    // DTO para respuesta de login
    public class LoginResponse
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public TipoRol Rol { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    // DTO para registro
    public class RegistroUsuarioRequest
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public TipoRol Rol { get; set; } = TipoRol.Cajero;
    }

    public class CambiarPasswordRequest
    {
        [Required]
        public string PasswordActual { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string PasswordNueva { get; set; } = string.Empty;
    }
}