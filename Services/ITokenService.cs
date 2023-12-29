namespace CatalogMinimalAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, int expireMinutes = 60);
    }
}
