using storeBack.Models;
using storeBack.ViewModels;

namespace storeBack.Services.Cliente
{
    public interface IClienteService
    {
        Task<TableResponse<ClienteDto>> getAllClientsAsync(int offset, int limit);
        Task<ClienteDto> getClientByIdAsync(int id);
        Task CreateClientAsync(Models.Cliente cliente);
        Task UpdateClientAsync(int id, UpdateClienteDto cliente);
        Task DeleteClientAsync(int id);
    }
}
