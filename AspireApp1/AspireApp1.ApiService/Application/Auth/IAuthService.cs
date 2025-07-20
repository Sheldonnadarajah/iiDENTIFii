using System.Threading.Tasks;

namespace AspireApp1.ApiService.Application.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, string Error)> RegisterAsync(RegisterCommand request);
        Task<(bool Success, string TokenOrError)> LoginAsync(LoginCommand request);
    }
}
