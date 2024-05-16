using Infrastructure.ExternalApis.Azure;
using Infrastructure.ExternalApis.Azure.Http;
using Infrastructure.ExternalApis.Azure.Settings;

using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Tests.Infrastructure.ExternalApis.Azure.Client;

public class AzureApiHttpClientTests
{
    private const string VALID_CONNECTION_STRING =
        "DefaultEndpointsProtocol=https;AccountName=any;AccountKey=key;EndpointSuffix=core.windows.net";

    private const string ANY_FILE_NAME = "file-name";
    private const string ANY_FILE_CONTENT_TYPE = "image/png";

    private readonly AzureApiHttpClient _azureApiHttpClient;
    private readonly Mock<IOptions<AzureApiSettings>> _azureSettings;
    private readonly Mock<IAzureBlobWrapper> _azureWrapper;
    private readonly byte[] _fileStream = new byte[5];
    private readonly Uri _uri = new("http://example.com/550e8400-e29b-41d4-a716-446655440000");

    public AzureApiHttpClientTests()
    {
        _azureSettings = new Mock<IOptions<AzureApiSettings>>();
        _azureWrapper = new Mock<IAzureBlobWrapper>();
        _azureApiHttpClient = new AzureApiHttpClient(_azureWrapper.Object);
    }

    private void ConfigureAzureStorageAccount()
    {
        Mock<CloudBlockBlob> cloudBlockBlob = new(_uri);
        _azureWrapper
            .Setup(x => x.GetCloudBlockBlob(It.IsAny<string>()))
            .ReturnsAsync(cloudBlockBlob.Object);
    }

    [Fact]
    public async Task GivenRequestToAzureWasSuccessful_WhenUploadFileAsync_ThenFileAbsoluteUri()
    {
        // Arrange
        AzureApiSettings settings = new() { ConnectionString = VALID_CONNECTION_STRING };
        _azureSettings
            .Setup(x => x.Value)
            .Returns(settings);

        ConfigureAzureStorageAccount();

        // Act
        string absoluteUri = await _azureApiHttpClient.UploadFileAsync(
            ANY_FILE_NAME,
            _fileStream,
            ANY_FILE_CONTENT_TYPE);

        // Assert
        absoluteUri.ShouldNotBeEmpty();
        absoluteUri.ShouldBe(_uri.AbsoluteUri);
    }
}