using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(15)]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200)]
        public string? Direccion { get; set; }

        [JsonIgnore] // Se asigna automáticamente
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int PuntosAcumulados { get; set; } = 0;
    }
}