using Microsoft.EntityFrameworkCore;
using storeBack.Services.ArticulosCliente;
using storeBack.ViewModels;

namespace storeBack.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {

        private readonly ApplicationDbContext _context;
        private readonly IArticuloClienteService _articuloClienteService;

        public ShoppingCartService(ApplicationDbContext context, IArticuloClienteService articuloClienteService)
        {
            _context = context;
            _articuloClienteService = articuloClienteService;
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
                    Id = ac.Id
                })
                .ToListAsync();

            return articles;
        }

        public async Task<List<ShoppingCartItemDto>> addShopingCartItem(int clientId, ShoppingCartItemModel articulo)
        {

            ArticuloClienteDto relation = new ArticuloClienteDto
            {
                ClienteId = clientId,
                ArticuloId = articulo.ArticuloId,
                Cantidad = articulo.Cantidad,
            };

            await _articuloClienteService.CreateAsync(relation);

            var articles = await _context.ArticuloCliente
                .Include(ac => ac.Articulo)
                .Where(ac => ac.ClienteId == clientId)
                .OrderBy(ac => ac.ArticuloId)
                .Select(ac => new ShoppingCartItemDto
                {
                    Cantidad = ac.Cantidad,
                    Articulo = ac.Articulo,
                    Id = ac.Id
                })
                .ToListAsync();

            return articles;
        }

        public async Task<List<ShoppingCartItemDto>> removeShopingCartItem(int clientId, ShoppingCartItemModel articulo)
        {

            ArticuloClienteTableDto relation = await _articuloClienteService.getByIdAsync((int)articulo.Id);

            var relationInfo = new ArticuloClienteDto
            {
                ArticuloId = relation.ArticuloId,
                ClienteId = relation.ArticuloId,
                Cantidad = relation.Cantidad - articulo.Cantidad,
            };

            if(relationInfo.Cantidad <= 0)
            {
                await _articuloClienteService.DeleteAsync((int)articulo.Id);
            } else
            { 
                await _articuloClienteService.UpdateAsync((int)articulo.Id, relationInfo);
            }


            var articles = await _context.ArticuloCliente
                .Include(ac => ac.Articulo)
                .Where(ac => ac.ClienteId == clientId)
                .OrderBy(ac => ac.ArticuloId)
                .Select(ac => new ShoppingCartItemDto
                {
                    Cantidad = ac.Cantidad,
                    Articulo = ac.Articulo,
                    Id = ac.Id,

                })
                .ToListAsync();

            return articles;
        }

        public async Task clearShoppingCart(int clientId)
        {
            await _articuloClienteService.DeleteByClientIdAsync(clientId);
        }

        public async Task buyShoppingCart(int clientId)
        {
            var shoppingCartItems = await _articuloClienteService.GetByClientIdAsync(clientId);

            foreach (var item in shoppingCartItems)
            {
                var article = item.Articulo;
                if (article != null && article.Stock < item.Cantidad)
                {
                    throw new InvalidOperationException($"No hay suficiente stock disponible para el artículo con ID {article.Id}");
                }
            }

            foreach (var item in shoppingCartItems)
            {
                var article = item.Articulo;
                if(article != null)
                {
                    article.Stock -= item.Cantidad;
                }

                _context.ArticuloCliente.Remove(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}
