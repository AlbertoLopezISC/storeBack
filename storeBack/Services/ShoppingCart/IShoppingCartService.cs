namespace storeBack.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartItemDto>> getShoppingCartClient(int clientId);

    }
}
