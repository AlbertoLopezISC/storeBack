using Microsoft.AspNetCore.Mvc;
using storeBack.Services.Articulos;
using System.Diagnostics;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly IArticuloService _articuloService;

        public ArticuloController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles(int offset = 0, int limit = 10)
        {
            var clients = await _articuloService.getAllArticlesAsync(offset, limit);
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var client = await _articuloService.getArticleByIdAsync(id);
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
        public async Task<IActionResult> CreateArticle(ArticuloDto article)
        {
            var data = await _articuloService.CreateArticleAsync(article);
            return CreatedAtAction(nameof(GetArticleById), new { id = data.Id }, article);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle(int id, ArticuloDto article)
        {
            var articleInfo = await _articuloService.getArticleByIdAsync(id);

            if (articleInfo == null)
            {
                return NotFound($"No se encontro un articulo con el ID {id}");
            }

            await _articuloService.UpdateArticleAsync(id, article);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articuloService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}
