
using Microsoft.EntityFrameworkCore;
using storeBack.Models;
using storeBack.ViewModels;

namespace storeBack.Services.Cliente
{
    public class ClienteService : IClienteService
    {

        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateClientAsync(Models.Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateClientAsync(int id, UpdateClienteDto cliente)
        {
            var clientInfo = await _context.Cliente.FindAsync(id);

            clientInfo.Nombre = cliente.Nombre;
            clientInfo.Apellidos = cliente.Apellidos;
            clientInfo.Direccion = cliente.Direccion;
            clientInfo.Email = cliente.Email;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            Models.Cliente cliente = await _context.Cliente.FindAsync(id);
            if(cliente != null)
            {
                _context.Cliente.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TableResponse<ClienteDto>> getAllClientsAsync(int offset, int limit)
        {
            var totalClients = await _context.Cliente.CountAsync();

            var clients = await _context
                .Cliente.OrderBy(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellidos = c.Apellidos,
                    Direccion = c.Direccion,
                    Email = c.Email,
                })
                .ToListAsync();

            var response = new TableResponse<ClienteDto>
            {
                Count = totalClients,
                Results = clients
            };

            return response;
        }

        public async Task<ClienteDto> getClientByIdAsync(int id)
        {
            var client = await _context.Cliente.FindAsync(id);
            
            if(client == null)
            {
                return null;
            }

            var clientDto = new ClienteDto
            {
                Id = client.Id,
                Nombre = client.Nombre,
                Apellidos = client.Apellidos,
                Direccion = client.Direccion,
                Email = client.Email,
            };

            return clientDto;
        }
    }
}
