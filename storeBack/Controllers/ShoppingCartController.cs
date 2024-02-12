using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using storeBack.Services.Cliente;
using storeBack.Services.ShoppingCart;
using System.Security.Claims;

namespace storeBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {

        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var articles = await _shoppingCartService.getShoppingCartClient(userId);
            return Ok(articles);
        }
    }
}
