namespace storeBack.Services.Tienda
{
    public class TiendaDto
    {
        public int Id { get; set; }
        public string Sucursal { get; set; }
        public string Direccion { get; set; }
    }

    public class UpdateTiendaDto
    { 
        public string Sucursal { get; set; }
        public string Direccion { get; set; }

    }
}
