using storeBack.ViewModels;

namespace storeBack.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartItemDto>> getShoppingCartClient(int clientId);
        Task<List<ShoppingCartItemDto>> addShopingCartItem(int clientId, ShoppingCartItemModel articuloId);
        Task<List<ShoppingCartItemDto>> removeShopingCartItem(int clientId, ShoppingCartItemModel articuloId);
        Task clearShoppingCart(int clientId);
        Task buyShoppingCart(int clientId);

    }
}
