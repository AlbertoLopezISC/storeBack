using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using storeBack.Models;
using storeBack.Services.Auth;
using storeBack.Services.Cliente;
using storeBack.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace storeBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClienteService _clienteService;

        public AuthController(IAuthService authService, IClienteService clienteService)
        {
            _authService = authService;
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel credential)
        {
            try
            {
                var user = await _clienteService.getClientByEmailAsync(credential.Email);
                ClienteDto clienteDto = new ClienteDto { Id = user.Id, Nombre = user.Nombre, Apellidos = user.Apellidos, Direccion = user.Direccion, Email = user.Email };
                if (user != null && user.Contraseña == credential.Password)
                {
                    return Ok(new
                    {
                        token = _authService.GenerateToken(clienteDto),
                        user = clienteDto,
                    });
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel data)
        {

            try
            {
                var principal = _authService.GetPrincipalFromToken(data.OldToken);
                if (principal == null)
                {
                    return BadRequest("Invalid token");
                }
                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var user = await _clienteService.getClientByIdAsync(userId);
                if (user != null)
                {
                    return Ok(new
                    {
                        oldToken = data.OldToken,
                        newToken = _authService.GenerateToken(user)
                    });
                }
                return BadRequest("Invalid token");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] AuthValidateTokenModel data)
        {
            var principal = _authService.GetPrincipalFromToken(data.Token);
            if (principal == null)
            {
                return BadRequest(new
                {
                    ValidToken = false
                });
            }
            else
            {
                return Ok(new
                {
                    ValidToken = true
                });
            }

        }



    }
}
