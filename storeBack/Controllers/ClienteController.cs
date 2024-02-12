using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeBack.Services.Cliente;
using System.Security.Claims;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clientService;
        
        public ClienteController(IClienteService clienteService)
        {
            _clientService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients(int offset = 0, int limit = 10)
        {
            var clients = await _clientService.getAllClientsAsync(offset, limit);
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _clientService.getClientByIdAsync(id);
            if(client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(Models.Cliente cliente)
        {
            await _clientService.CreateClientAsync(cliente);
            return CreatedAtAction(nameof(GetClientById), new {id = cliente.Id}, cliente);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(int id, UpdateClienteDto cliente)
        {
            var clientInfo = await _clientService.getClientByIdAsync(id);

            if(clientInfo == null)
            {
                return NotFound($"No se encontro un cliente con el ID {id}");
            }

            await _clientService.UpdateClientAsync(id, cliente);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }

    }
}
