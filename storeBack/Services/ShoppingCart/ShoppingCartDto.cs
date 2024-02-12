
using storeBack.Models;

namespace storeBack.Services.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public Articulo Articulo { get; set; }
    }
}
