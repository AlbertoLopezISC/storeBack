using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using storeBack.Services.Articulos;
using storeBack.Services.ArticulosCliente;
using storeBack.Services.ArticulosTienda;
using storeBack.Services.Cliente;
using storeBack.Services.Tienda;
using System.Diagnostics;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloTiendaController : ControllerBase
    {
        private readonly IArticuloTiendaService _service;
        private readonly ITiendaService _serviceTienda;
        private readonly IArticuloService _serviceArticulo;

        public ArticuloTiendaController(IArticuloTiendaService service, ITiendaService tiendaService, IArticuloService articuloService)
        {
            _service = service;
            _serviceTienda = tiendaService;
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
        public async Task<IActionResult> Create(ArticuloTiendaDto relation)
        {
            var store = await _serviceTienda.getStoreByIdAsync(relation.TiendaId);
            var article = await _serviceArticulo.getArticleByIdAsync(relation.ArticuloId);

            if (store == null || article == null)
            {
                return BadRequest(new
                {
                    title = "Not Found",
                    status = 404,
                    message = $"{(store == null ? $"Tienda " : "")}{(store == null && article == null ? $"y " : "")}{(article == null ? $"Articulo" : "")} no encontrado",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var data = await _service.CreateAsync(relation);
            return CreatedAtAction(nameof(GetById), new { id = data.Id }, relation);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, ArticuloTiendaDto relation)
        {
            var dataInfo = await _service.getByIdAsync(id);

            if (dataInfo == null)
            {
                return NotFound($"No se encontro una relacion con el ID {id}");
            }

            var store = await _serviceTienda.getStoreByIdAsync(relation.TiendaId);
            var article = await _serviceArticulo.getArticleByIdAsync(relation.ArticuloId);

            if (store == null || article == null)
            {
                return BadRequest(new
                {
                    title = "Not Found",
                    status = 404,
                    message = $"{(store == null ? $"Tienda " : "")}{(store == null && article == null ? $"y " : "")}{(article == null ? $"Articulo" : "")} no encontrado",
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
