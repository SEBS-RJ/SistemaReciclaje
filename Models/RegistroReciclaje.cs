using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class RegistroReciclaje
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(0.1, 1000)]
        public double CantidadKg { get; set; }

        public int PuntosObtenidos { get; set; }

        // Foreign Keys
        public int Id_Usuario { get; set; }
        [ForeignKey(nameof(Id_Usuario))]
        [JsonIgnore] // No mostrar objeto completo en JSON
        public Usuario? Usuario { get; set; }

        public int Id_Material { get; set; }
        [ForeignKey(nameof(Id_Material))]
        [JsonIgnore] // No mostrar objeto completo en JSON
        public Material? Material { get; set; }

        public int Id_PuntoVerde { get; set; }
        [ForeignKey(nameof(Id_PuntoVerde))]
        [JsonIgnore] // No mostrar objeto completo en JSON
        public PuntoVerde? PuntoVerde { get; set; }
    }
}