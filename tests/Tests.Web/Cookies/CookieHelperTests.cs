using Microsoft.AspNetCore.Http;

using Web.Cookies;

namespace Tests.Web.Cookies;

public class CookieHelperTests
{
    private const string ANY_COOKIE_NAME = "lang";
    private const string ANY_COOKIE_VALUE = "fr";

    private readonly HttpContext _httpContext = new DefaultHttpContext();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenNullEmptyOrWhiteSpaceCookieName_WhenGetCookieValue_ThenReturnEmpty(string? cookieName)
    {
        // Act
        string cookieValue = _httpContext.GetCookieValue(cookieName!);

        // Assert
        cookieValue.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenNullEmptyOrWhiteSpaceCookieName_WhenSetCookieValue_ThenDontAddCookie(string? cookieName)
    {
        // Act
        _httpContext.SetCookieValue(cookieName!, ANY_COOKIE_VALUE, false, false);

        // Assert
        _httpContext.Response.Cookies.ShouldNotBeNull();
    }

    [Fact]
    public void WhenGetCookieValue_ThenReturnValue()
    {
        // Act
        string cookieValue = _httpContext.GetCookieValue(ANY_COOKIE_NAME);

        // Assert
        cookieValue.ShouldBeEmpty();
    }

    [Fact]
    public void WhenSetCookieValue_ThenAddCookieValueToResponse()
    {
        // Act
        _httpContext.SetCookieValue(ANY_COOKIE_NAME, ANY_COOKIE_VALUE, true, true);

        // Assert
        _httpContext.Response.Cookies.ShouldNotBeNull();
    }
}