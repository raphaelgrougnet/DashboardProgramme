using FluentValidation.TestHelper;

using Web.Features.Admins.Books.GetBook;

namespace Tests.Web.Features.Admins.Books.GetBook;

public class GetBookValidatorTests
{
    private readonly GetBookRequest _request = new() { Id = Guid.NewGuid() };
    private readonly GetBookValidator _validator = new();

    [Fact]
    public void GivenEmptytId_WhenValidate_ThenReturnError()
    {
        // Arrange
        _request.Id = Guid.Empty;

        // Act
        TestValidationResult<GetBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void GivenValidRequest_WhenValidate_ThenReturnNoErrors()
    {
        // Act
        TestValidationResult<GetBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}