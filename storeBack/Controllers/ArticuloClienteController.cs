using Microsoft.AspNetCore.Mvc;
using storeBack.Models;
using storeBack.Services.Articulos;
using storeBack.Services.ArticulosCliente;
using storeBack.Services.Cliente;
using System.Diagnostics;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloClienteController : ControllerBase
    {
        private readonly IArticuloClienteService _service;
        private readonly IClienteService _serviceCliente;
        private readonly IArticuloService _serviceArticulo;

        public ArticuloClienteController(IArticuloClienteService service, IClienteService clienteService, IArticuloService articuloService)
        {
            _service = service;
            _serviceCliente = clienteService;
            _serviceArticulo = articuloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 10)
        {
            var clients = await _service.getAllAsync(offset, limit);
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.getByIdAsync(id);
            if (client == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    message = "Articulo no encontrado",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticuloClienteDto relation)
        {
            var client = await _serviceCliente.getClientByIdAsync(relation.ClienteId);
            var article = await _serviceArticulo.getArticleByIdAsync(relation.ArticuloId);

            if(client == null || article == null)
            {
                return BadRequest(new
                {
                    title = "Not Found",
                    status = 404,
                    message = $"{(client == null ? $"Cliente " : "")}{(client == null && article == null ? $"y " : "")}{(article == null ? $"Articulo" : "")} no encontrado",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var data = await _service.CreateAsync(relation);
            return CreatedAtAction(nameof(GetById), new { id = data.Id }, relation);

        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, ArticuloClienteDto relation)
        {
            var dataInfo = await _service.getByIdAsync(id);

            if (dataInfo == null)
            {
                return NotFound($"No se encontro una relacion con el ID {id}");
            }

            var client = await _serviceCliente.getClientByIdAsync(relation.ClienteId);
            var article = await _serviceArticulo.getArticleByIdAsync(relation.ArticuloId);

            if (client == null || article == null)
            {
                return BadRequest(new
                {
                    title = "Not Found",
                    status = 404,
                    message = $"{(client == null ? $"Cliente " : "")}{(client == null && article == null ? $"y " : "")}{(article == null ? $"Articulo" : "")} no encontrado",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            await _service.UpdateAsync(id, relation);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
