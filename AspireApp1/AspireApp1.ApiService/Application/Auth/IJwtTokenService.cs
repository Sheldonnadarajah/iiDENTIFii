using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Application.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
