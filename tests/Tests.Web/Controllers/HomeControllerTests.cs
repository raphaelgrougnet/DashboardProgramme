using System.Security.Claims;

using Application.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.Controllers;

namespace Tests.Web.Controllers;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _logger = new();

    private HomeController _homeController;

    public HomeControllerTests()
    {
        _homeController = new HomeController(_logger.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };
    }

    private void BuildHomeController(bool withAuthenticatedUser = false)
    {
        ClaimsPrincipal claimsPrincipal = withAuthenticatedUser
            ? new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>(), CookieAuthenticationDefaults.AuthenticationScheme).IntoList())
            : new ClaimsPrincipal();
        _homeController = new HomeController(_logger.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            }
        };
    }

    [Fact]
    public void GivenAuthenticatedMember_WhenGetIndex_ThenReturnVueAppPageView()
    {
        // Arrange
        BuildHomeController(true);

        // Act
        IActionResult actionResult = _homeController.Index();

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Index");
        redirectToActionResult.ControllerName.ShouldBe("VueApp");
    }

    [Fact]
    public void GivenNobodyIsAuthenticated_WhenGetIndex_ThenRedirectToLogin()
    {
        // Arrange
        BuildHomeController();

        // Act
        IActionResult actionResult = _homeController.Index();

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Login");
        redirectToActionResult.ControllerName.ShouldBe("Authentication");
    }
}