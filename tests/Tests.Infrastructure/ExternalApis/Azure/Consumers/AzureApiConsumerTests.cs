﻿using Infrastructure.ExternalApis.Azure.Consumers;
using Infrastructure.ExternalApis.Azure.Exceptions;
using Infrastructure.ExternalApis.Azure.Http;

using Microsoft.AspNetCore.Http;

namespace Tests.Infrastructure.ExternalApis.Azure.Consumers;

// ReSharper disable InconsistentNaming
public class AzureApiConsumerTests
{
    private const string ANY_FILE_URI = "any-file-uri";
    private const string ANY_FILE_NAME = "any-file-name";
    private const string ANY_FILE_FILENAME = "any-file-filename";
    private readonly Mock<IAzureApiHttpClient> _azureHttpClient;
    private readonly AzureBlobApiConsumer _fileStorageApiConsumer;

    public AzureApiConsumerTests()
    {
        _azureHttpClient = new Mock<IAzureApiHttpClient>();
        _fileStorageApiConsumer = new AzureBlobApiConsumer(_azureHttpClient.Object);
    }

    private IFormFile BuildInvalidOrValidFormFile(bool buildValidForm = false)
    {
        FormFile formFile = new(
            new MemoryStream(),
            200,
            200,
            ANY_FILE_NAME,
            ANY_FILE_FILENAME);

        if (buildValidForm)
        {
            formFile.Headers = new HeaderDictionary { new("Content-Type", "image/png") };
        }

        return formFile;
    }

    [Fact(Skip = "This logic is commented out until students decide to implement it.")]
    public async Task GivenRequestToCapsuleWasNotSuccessful_WhenUploadFileAsync_ThenThrowAzureApiException()
    {
        // Arrange
        IFormFile file = BuildInvalidOrValidFormFile();
        _azureHttpClient
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(ANY_FILE_URI);

        // Act & assert
        AzureApiException exception = await Assert.ThrowsAsync<AzureApiException>(async () =>
            await _fileStorageApiConsumer.UploadFileAsync(file));
        exception.Message.ShouldStartWith("Invalid file");
    }

    [Fact]
    public async Task GivenRequestToCapsuleWasSuccessful_WhenUploadFileAsync_ThenFileAbsoluteUri()
    {
        // Arrange
        IFormFile file = BuildInvalidOrValidFormFile(true);
        _azureHttpClient
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(ANY_FILE_URI);

        // Act
        string response = await _fileStorageApiConsumer.UploadFileAsync(file);

        // Assert
        response.ShouldNotBeNullOrEmpty();
        response.ShouldBe(ANY_FILE_URI);
    }
}