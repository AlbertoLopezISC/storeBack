namespace storeBack.Services.ArticulosCliente
{
    public class ArticuloClienteDto
    {
        public int ArticuloId { get; set; }
        public int ClienteId { get; set; }
        public int Cantidad { get; set; }
    }

    public class ArticuloClienteTableDto
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public int ClienteId { get; set; }
        public int Cantidad { get; set; }
    }
}
