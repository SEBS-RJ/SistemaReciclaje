using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class PuntoVerde
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200)]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "La latitud es obligatoria")]
        public double Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria")]
        public double Longitud { get; set; }

        [StringLength(20)]
        public string? Horario { get; set; }

        [JsonIgnore] // Se asigna por defecto como true
        public bool Activo { get; set; } = true;
    }
}