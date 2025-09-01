using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaReciclaje.Models
{
    public class CanjeBeneficio
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCanje { get; set; }

        public int PuntosUtilizados { get; set; }

        public bool Utilizado { get; set; } = false;

        // Foreign Keys
        public int Id_Usuario { get; set; }
        [ForeignKey(nameof(Id_Usuario))]
        [JsonIgnore] // No mostrar objeto completo en JSON
        public Usuario? Usuario { get; set; }

        public int Id_Beneficio { get; set; }
        [ForeignKey(nameof(Id_Beneficio))]
        [JsonIgnore] // No mostrar objeto completo en JSON
        public Beneficio? Beneficio { get; set; }
    }
}