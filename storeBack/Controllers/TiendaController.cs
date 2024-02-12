using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using storeBack.Services.Cliente;
using storeBack.Services.Tienda;
using System.Diagnostics;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendaController : ControllerBase
    {
        private readonly ITiendaService _tiendaService;

        public TiendaController(ITiendaService clienteService)
        {
            _tiendaService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStores(int offset = 0, int limit = 10)
        {
            var clients = await _tiendaService.getAllStoreAsync(offset, limit);
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var client = await _tiendaService.getStoreByIdAsync(id);
            if (client == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    message = "Tienda no encontrada",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(Models.Tienda store)
        {
            await _tiendaService.CreateStoreAsync(store);
            return CreatedAtAction(nameof(GetStoreById), new { id = store.Id }, store);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStore(int id, UpdateTiendaDto store)
        {
            var storeInfo = await _tiendaService.getStoreByIdAsync(id);

            if (storeInfo == null)
            {
                return NotFound($"No se encontro una tienda con el ID {id}");
            }

            await _tiendaService.UpdateStoreAsync(id, store);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            await _tiendaService.DeleteStoreAsync(id);
            return NoContent();
        }
    }
}
