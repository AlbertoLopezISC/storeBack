using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using storeBack.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel credential)
        {
            try
            {
                var user = await _context.Cliente.FirstOrDefaultAsync(c => c.Email == credential.Email);
                if (user != null && user.Contraseña == credential.Password)
                {
                    return Ok(new
                    {
                        token = GenerateToken(user)
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
        public async Task<IActionResult> RefreshToken([FromBody] string oldToken)
        {

            try
            {
                var principal = GetPrincipalFromToken(oldToken);
                if (principal == null)
                {
                    return BadRequest("Invalid token");
                }
                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var user = await _context.Cliente.FirstOrDefaultAsync(c => c.Id == userId);
                if (user != null)
                {
                    return Ok(new
                    {
                        oldToken = oldToken,
                        newToken = GenerateToken(user)
                    });
                }
                return BadRequest("Invalid token");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GenerateToken(Cliente user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MyApp",
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: claims,
                signingCredentials: creds,
                audience: "MyApp"
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidAudience = "MyApp",
                    ValidIssuer = "MyApp",
                    ValidateLifetime = false
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Token inválido");
                }

                return principal;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
