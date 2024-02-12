using storeBack.Models;
using System.ComponentModel.DataAnnotations;

namespace storeBack.Services.Cliente
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
    }

    public class UpdateClienteDto
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
    }

}
