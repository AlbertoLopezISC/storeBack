using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace storeBack.Models
{
    public class Articulo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        [Required, MaxLength(255)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public double Precio { get; set; }
        [Required, MaxLength(120)]
        public string ImgUrl { get; set; } = string.Empty;
        public int Stock { get; set; }

    }
}
