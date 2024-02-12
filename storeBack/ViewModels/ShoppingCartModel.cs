namespace storeBack.ViewModels
{
    public class ShoppingCartItemModel
    {
        public int? Id { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }
        public bool AgregarFlag { get; set; }
    }
}
