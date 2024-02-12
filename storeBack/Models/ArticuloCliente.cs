using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace storeBack.Models
{
    public class ArticuloCliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set;}
        public int Cantidad { get; set; }

    }
}
