using System.Security.Claims;
using AspireApp1.ApiService.Application.Auth;
using FluentAssertions;
using NUnit.Framework;

namespace AspireApp1.Tests.Application.Auth
{
    [TestFixture]
    public class AuthUtilsTests
    {
        [Test]
        public void GetUserIdFromClaims_ShouldReturnUserId_WhenValidClaimExists()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "42") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = AuthUtils.GetUserIdFromClaims(principal);

            // Assert
            result.Should().Be(42);
        }

        [Test]
        public void GetUserIdFromClaims_ShouldReturnNull_WhenNoNameIdentifierClaim()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Email, "test@example.com") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = AuthUtils.GetUserIdFromClaims(principal);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetUserIdFromClaims_ShouldReturnNull_WhenClaimIsNotInt()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "notanint") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = AuthUtils.GetUserIdFromClaims(principal);

            // Assert
            result.Should().BeNull();
        }
    }
}
