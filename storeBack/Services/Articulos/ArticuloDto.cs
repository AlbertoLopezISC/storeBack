
namespace storeBack.Services.Articulos
{
    public class ArticuloDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double Precio { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
