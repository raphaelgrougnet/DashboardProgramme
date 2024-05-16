using FluentValidation.TestHelper;

using Web.Features.Admins.Books.DeleteBook;

namespace Tests.Web.Features.Admins.Books.DeleteBook;

public class DeleteBookValidatorTests
{
    private readonly DeleteBookRequest _request = new() { Id = Guid.NewGuid() };
    private readonly DeleteBookValidator _validator = new();

    [Fact]
    public void GivenEmptyId_WhenValidate_ThenReturnError()
    {
        // Arrange
        _request.Id = Guid.Empty;

        // Act
        TestValidationResult<DeleteBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void GivenValidRequest_WhenValidate_ThenReturnNoErrors()
    {
        // Act
        TestValidationResult<DeleteBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}