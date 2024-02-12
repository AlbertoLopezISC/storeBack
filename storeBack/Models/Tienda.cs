using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace storeBack.Models
{
    public class Tienda
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Sucursal { get; set; } = string.Empty;
        [Required]
        public string Direccion { get; set; } = string.Empty;

    }
}
