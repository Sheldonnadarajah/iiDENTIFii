namespace AspireApp1.ApiService.Application.Auth
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
