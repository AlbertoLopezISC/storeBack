using Microsoft.EntityFrameworkCore;

namespace storeBack.Services.ShoppingCart
{
    public class ShoppingCartService: IShoppingCartService
    {

        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingCartItemDto>> getShoppingCartClient(int clientId)
        {
            var articles = await _context.ArticuloCliente
                .Include(ac => ac.Articulo)
                .Where(ac => ac.ClienteId == clientId)
                .OrderBy(ac => ac.ArticuloId)
                .Select(ac => new ShoppingCartItemDto
                {
                    Cantidad = ac.Cantidad,
                    Articulo = ac.Articulo,

                })
                .ToListAsync();

            return articles;
        }
    }
}
