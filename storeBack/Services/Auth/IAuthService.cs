using storeBack.Services.Cliente;
using System.Security.Claims;

namespace storeBack.Services.Auth
{
    public interface IAuthService
    {
        string GenerateToken(ClienteDto user);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
