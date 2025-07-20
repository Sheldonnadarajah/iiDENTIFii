using System.Threading.Tasks;
using AspireApp1.ApiService.Application.Auth;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AspireApp1.Tests.Application.Auth
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private IAuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
        }

        [Test]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User { Id = 1, Username = "test", PasswordHash = "hashed" };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("test")).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword("password", "hashed")).Returns(true);
            _jwtTokenServiceMock.Setup(j => j.GenerateToken(user)).Returns("token");

            // Act
            var (success, token) = await _authService.LoginAsync(new LoginCommand { Username = "test", Password = "password" });

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                success.Should().BeTrue();
                token.Should().Be("token");
            }
        }

        [Test]
        public async Task LoginAsync_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("nouser")).ReturnsAsync((User?)null);

            // Act
            var (success, error) = await _authService.LoginAsync(new LoginCommand { Username = "nouser", Password = "pass" });

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                success.Should().BeFalse();
                error.Should().Be("Invalid username or password.");
            }
        }

        [Test]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsNew()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User)null!);
            _passwordHasherMock.Setup(h => h.HashPassword("pass")).Returns("hashed");
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var (success, error) = await _authService.RegisterAsync(new RegisterCommand { Username = "newuser", Password = "pass", Email = "e@e.com" });

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                success.Should().BeTrue();
                error.Should().Be("User registered successfully.");
            }
        }

        [Test]
        public async Task RegisterAsync_ShouldReturnError_WhenUsernameExists()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("exists")).ReturnsAsync(new User());

            // Act
            var (success, error) = await _authService.RegisterAsync(new RegisterCommand { Username = "exists", Password = "pass", Email = "e@e.com" });

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                success.Should().BeFalse();
                error.Should().Be("Username already exists.");
            }
        }
    }
}
