using System.Text;

using Application.Services.Notifications.Dtos;

using AutoMapper;

using Infrastructure.Mailing.Mapping;

using SendGrid.Helpers.Mail;

using Tests.Common.Mapping;

namespace Tests.Infrastructure.Mailing;

public class MailingMappingProfileTests
{
    private const string ANY_CONTENT_TYPE = "image/png";
    private const string ANY_FILE_NAME = "image.png";
    private const string ANY_BODY_STREAM = "eyuhwvo48wvnhfvebjntvw";

    private readonly IMapper _mapper = new MapperBuilder().WithProfile<MailingMappingProfile>().Build();

    [Fact]
    public void GivenAttachmentDtoWithAnyBodyStream_WhenMap_ThenReturnAttachementWithBase64Content()
    {
        // Arrange
        AttachmentDto attachementDto = new() { BodyStream = ANY_BODY_STREAM };

        // Act
        Attachment? actual = _mapper.Map<Attachment>(attachementDto);

        // Assert
        actual.Content.ShouldBe(Convert.ToBase64String(Encoding.UTF8.GetBytes(ANY_BODY_STREAM)));
    }

    [Fact]
    public void GivenAttachmentDtoWithAnyContentType_WhenMap_ThenReturnAttachementWithSameContentType()
    {
        // Arrange
        AttachmentDto attachementDto = new() { ContentType = ANY_CONTENT_TYPE };

        // Act
        Attachment? actual = _mapper.Map<Attachment>(attachementDto);

        // Assert
        actual.Type.ShouldBe(ANY_CONTENT_TYPE);
    }

    [Fact]
    public void GivenAttachmentDtoWithAnyFileName_WhenMap_ThenReturnAttachementWithSameFileName()
    {
        // Arrange
        AttachmentDto attachementDto = new() { FileName = ANY_FILE_NAME };

        // Act
        Attachment? actual = _mapper.Map<Attachment>(attachementDto);

        // Assert
        actual.Filename.ShouldBe(ANY_FILE_NAME);
    }

    [Fact]
    public void WhenMap_ThenReturnAttachementWithDispositionSetToAttachment()
    {
        // Arrange
        AttachmentDto attachementDto = new();

        // Act
        Attachment? actual = _mapper.Map<Attachment>(attachementDto);

        // Assert
        actual.Disposition.ShouldBe("attachment");
    }
}