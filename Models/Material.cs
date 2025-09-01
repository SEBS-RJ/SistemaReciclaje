using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del material es obligatorio")]
        [StringLength(50, MinimumLength = 2)]
        public string? Nombre { get; set; }

        [StringLength(100)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Los puntos por kg son obligatorios")]
        [Range(1, 100)]
        public int PuntosPorKg { get; set; }

        [JsonIgnore] // Se asigna por defecto como true
        public bool Activo { get; set; } = true;
    }
}