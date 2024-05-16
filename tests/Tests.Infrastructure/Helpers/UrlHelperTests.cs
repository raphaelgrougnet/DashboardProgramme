using Infrastructure.Helpers;

namespace Tests.Infrastructure.Helpers;

public class UrlHelperTests
{
    private const string ANY_BASE_URL = "https://localhost:7101";

    [Fact]
    public void GivenNoQueryParameters_WhenBuildUriWithQueryParameters_ThenReturnBaseUrlOnly()
    {
        // Arrange
        List<KeyValuePair<string, string>> queryParameters = [];

        // Act
        string uri = UrlHelper.BuildUriWithQueryParameters(ANY_BASE_URL, queryParameters);

        // Assert
        uri.ShouldBe(ANY_BASE_URL);
    }

    [Fact]
    public void GivenQueryParameters_WhenBuildUriWithQueryParameters_ThenReturnUrlStartingWithBaseUrl()
    {
        // Arrange
        List<KeyValuePair<string, string>> queryParameters =
            [new KeyValuePair<string, string>("page", "0"), new KeyValuePair<string, string>("perPage", "100")];

        // Act
        string uri = UrlHelper.BuildUriWithQueryParameters(ANY_BASE_URL, queryParameters);

        // Assert
        uri.ShouldStartWith(ANY_BASE_URL);
    }

    [Fact]
    public void GivenQueryParameters_WhenBuildUriWithQueryParameters_TheReturnUrlDoesNotEndWithAmpersand()
    {
        // Arrange
        List<KeyValuePair<string, string>> queryParameters =
            [new KeyValuePair<string, string>("page", "0"), new KeyValuePair<string, string>("perPage", "100")];

        // Act
        string uri = UrlHelper.BuildUriWithQueryParameters(ANY_BASE_URL, queryParameters);

        // Assert
        uri.ShouldNotEndWith("&");
    }

    [Fact]
    public void GivenQueryParameters_WhenBuildUriWithQueryParameters_TheReturnUrlIncludesAllParameters()
    {
        // Arrange
        List<KeyValuePair<string, string>> queryParameters =
            [new KeyValuePair<string, string>("page", "0"), new KeyValuePair<string, string>("perPage", "100")];

        // Act
        string uri = UrlHelper.BuildUriWithQueryParameters(ANY_BASE_URL, queryParameters);

        // Assert
        uri.ShouldBe($"{ANY_BASE_URL}?page=0&perPage=100");
    }
}