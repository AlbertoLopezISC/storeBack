
using storeBack.Models;

namespace storeBack.Services.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public int Cantidad { get; set; }
        public Articulo Articulo { get; set; }
    }
}
