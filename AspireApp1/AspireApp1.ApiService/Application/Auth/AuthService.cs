using System.Linq;
using System.Security.Claims;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using System.Threading.Tasks;
namespace AspireApp1.ApiService.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(bool Success, string Error)> RegisterAsync(RegisterCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Email))
                return (false, "Username, password, and email are required.");

            var existing = await _userRepository.GetByUsernameAsync(request.Username);
            if (existing != null)
                return (false, "Username already exists.");

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };
            await _userRepository.AddAsync(user);
            return (true, "User registered successfully.");
        }

        public async Task<(bool Success, string TokenOrError)> LoginAsync(LoginCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return (false, "Username and password are required.");

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                return (false, "Invalid username or password.");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return (false, "Invalid username or password.");

            var token = _jwtTokenService.GenerateToken(user);
            return (true, token);
        }
    }
}
