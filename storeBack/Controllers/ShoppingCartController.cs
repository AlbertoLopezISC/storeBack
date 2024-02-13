using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using storeBack.Models;
using storeBack.Services.Articulos;
using storeBack.Services.Cliente;
using storeBack.Services.ShoppingCart;
using storeBack.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {

        private readonly IShoppingCartService _shoppingCartService;
        private readonly IClienteService _serviceCliente;
        private readonly IArticuloService _serviceArticulo;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IClienteService clienteService, IArticuloService articuloService)
        {
            _shoppingCartService = shoppingCartService;
            _serviceCliente = clienteService;
            _serviceArticulo = articuloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var articles = await _shoppingCartService.getShoppingCartClient(userId);
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult>PostAddShoppingCartItem([FromBody] ShoppingCartItemModel article)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var client = await _serviceCliente.getClientByIdAsync(userId);
            var articleInfo = await _serviceArticulo.getArticleByIdAsync(article.ArticuloId);

            if (client == null || articleInfo == null)
            {
                return BadRequest(new
                {
                    title = "Not Found",
                    status = 404,
                    message = $"{(client == null ? $"Cliente " : "")}{(client == null && articleInfo == null ? $"y " : "")}{(articleInfo == null ? $"Articulo" : "")} no encontrado",
                    traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }


            if (article.AgregarFlag)
            {
                    try
                    {
                        return Ok(await _shoppingCartService.addShopingCartItem(userId, article));
                    }catch (Exception ex)
                    {
                        return BadRequest(new
                        {
                            Status = 400,
                            Message = ex.Message
                        });
                    }
            }

            return Ok(await _shoppingCartService.removeShopingCartItem(userId, article));

        }

        [HttpPost("buy-shopping-cart")]
        public async Task<IActionResult> BuyShoppingCart()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            try
            {
                await _shoppingCartService.buyShoppingCart(userId);
            } catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = ex.Message,
                });
            }

            return Ok();

        }

        [HttpDelete("clear-shopping-cart")]
        public async Task<IActionResult> ClearShoppingCart()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            await _shoppingCartService.clearShoppingCart(userId);
            return Ok(new {
                Status = "Carrito borrado"
            });
        }

    }
}
