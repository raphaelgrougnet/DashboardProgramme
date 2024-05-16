using Application.Helpers;

namespace Tests.Application.Helpers;

public class UriHelperTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   \t")]
    [InlineData("abc")]
    [InlineData("123456")]
    [InlineData("000 000 0000")]
    [InlineData(null)]
    public void GivenInvalidUri_WhenIsValidUri_ThenReturnFalse(string uri)
    {
        // Act
        bool response = uri.IsValidUri();

        // Assert
        response.ShouldBeFalse();
    }

    [Theory]
    [InlineData("https://www.uri.com")]
    [InlineData("https://www.uri.ca")]
    [InlineData("http://www.uri.com")]
    public void GivenValidUri_WhenIsValidUri_ThenReturnTrue(string uri)
    {
        // Act
        bool response = uri.IsValidUri();

        // Assert
        response.ShouldBeTrue();
    }
}