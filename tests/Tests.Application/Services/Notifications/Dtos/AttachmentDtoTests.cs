using Application.Services.Notifications.Dtos;

namespace Tests.Application.Services.Notifications.Dtos;

public class AttachmentDtoTests
{
    private const string ANY_FILE_NAME = "file-name";
    private const string ANY_FILE_STREAM = "file-stream";
    private const string ANY_FILE_CONTENT_TYPE = "file-content-type";

    [Fact]
    public void GivenAnyContentType_WhenNewAttachmentDto_ThenContentTypeShouldBeSameAsGivenContentType()
    {
        // Act
        AttachmentDto attachmentDto = new() { ContentType = ANY_FILE_CONTENT_TYPE };

        // Assert
        attachmentDto.ContentType.ShouldBe(ANY_FILE_CONTENT_TYPE);
    }

    [Fact]
    public void GivenAnyFileName_WhenNewAttachmentDto_ThenFileNameShouldBeSameAsGivenFileName()
    {
        // Act
        AttachmentDto attachmentDto = new() { FileName = ANY_FILE_NAME };

        // Assert
        attachmentDto.FileName.ShouldBe(ANY_FILE_NAME);
    }

    [Fact]
    public void GivenAnyFileStream_WhenNewAttachmentDto_ThenBodyStreamShouldBeSameAsGivenBodyStream()
    {
        // Act
        AttachmentDto attachmentDto = new() { BodyStream = ANY_FILE_STREAM };

        // Assert
        attachmentDto.BodyStream.ShouldBe(ANY_FILE_STREAM);
    }
}