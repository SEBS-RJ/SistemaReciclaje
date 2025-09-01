using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class Beneficio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del beneficio es obligatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string? Nombre { get; set; }

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Los puntos requeridos son obligatorios")]
        [Range(1, 10000)]
        public int PuntosRequeridos { get; set; }

        [StringLength(50)]
        public string? TipoBeneficio { get; set; }

        [JsonIgnore] // Se asigna por defecto como true
        public bool Activo { get; set; } = true;
    }
}