using CatalogMinimalAPI.Models;

namespace CatalogMinimalAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, User user);
    }
}
