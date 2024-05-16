using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Tests.Web.TestCollections;

using Web.Extensions;

namespace Tests.Web.Extensions;

[Collection(WebTestCollection.NAME)]
public class HttpContextExtensionsTests
{
    [Fact]
    public void GivenHttpContextHasNoUser_WhenIsAuthenticated_ThenReturnFalse()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        // Act
        bool isAuthenticated = httpContext.IsAuthenticated();

        // Assert
        isAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public void GivenHttpContextHasUser_WhenIsAuthenticated_ThenReturnTrue()
    {
        // Arrange
        ClaimsIdentity claimsIdentity = new(new List<Claim>(), CookieAuthenticationDefaults.AuthenticationScheme);
        DefaultHttpContext httpContext = new() { User = new ClaimsPrincipal(claimsIdentity) };

        // Act
        bool isAuthenticated = httpContext.IsAuthenticated();

        // Assert
        isAuthenticated.ShouldBeTrue();
    }
}