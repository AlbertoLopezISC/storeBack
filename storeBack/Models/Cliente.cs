using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace storeBack.Models
{
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } =  string.Empty;
        [Required]
        public string Apellidos { get; set; } = string.Empty;
        [Required]
        public string Direccion {  get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Contraseña { get; set; } = string.Empty;


    }
}
