using FluentValidation.TestHelper;

using Web.Features.Admins.Books.EditBook;

namespace Tests.Web.Features.Admins.Books.EditBook;

public class EditBookValidatorTests
{
    private const string NAME_FR = "Guide de la réglementation en copropriété";
    private const string NAME_EN = "Guide to condominium regulations";
    private const string DESCRIPTION_FR = "On vise, en copropriété, à assurer aux occupant un milieu de vie paisible.";

    private const string DESCRIPTION_EN =
        "We aim, in co-ownership, to provide occupants with a peaceful living environment.";

    private const string ISBN = "978-2-89689-559-5";
    private const string AUTHOR = "Christine Gagnon, Yves Papineau";
    private const string EDITOR = "Wilson & Lafleur";
    private const int YEAR_OF_PUBLICATION = 2023;
    private const int NUMBER_OF_PAGES = 346;
    private const decimal PRICE = 20;

    private readonly EditBookRequest _request = new()
    {
        Id = Guid.NewGuid(),
        NameFr = NAME_FR,
        NameEn = NAME_EN,
        DescriptionFr = DESCRIPTION_FR,
        DescriptionEn = DESCRIPTION_EN,
        Isbn = ISBN,
        Author = AUTHOR,
        Editor = EDITOR,
        YearOfPublication = YEAR_OF_PUBLICATION,
        NumberOfPages = NUMBER_OF_PAGES,
        Price = PRICE
    };

    private readonly EditBookValidator _validator = new();

    [Fact]
    public void GivenEmptytId_WhenValidate_ThenReturnError()
    {
        // Arrange
        _request.Id = Guid.Empty;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceAuthor_WhenValidate_ThenReturnError(string? author)
    {
        // Arrange
        _request.Author = author!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Author);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceDescriptionEn_WhenValidate_ThenReturnError(string? descriptionEn)
    {
        // Arrange
        _request.DescriptionEn = descriptionEn!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.DescriptionEn);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceDescriptionFr_WhenValidate_ThenReturnError(string? descriptionFr)
    {
        // Arrange
        _request.DescriptionFr = descriptionFr!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.DescriptionFr);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceEditor_WhenValidate_ThenReturnError(string? editor)
    {
        // Arrange
        _request.Editor = editor!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Editor);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceIsbn_WhenValidate_ThenReturnError(string? isbn)
    {
        // Arrange
        _request.Isbn = isbn!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Isbn);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceNameEn_WhenValidate_ThenReturnError(string? nameEn)
    {
        // Arrange
        _request.NameEn = nameEn!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.NameEn);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GivenNullEmptyOrWhitespaceNameFr_WhenValidate_ThenReturnError(string? nameFr)
    {
        // Arrange
        _request.NameFr = nameFr!;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.NameFr);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void GivenNullEmptyOrWhitespaceNumberOfPages_WhenValidate_ThenReturnError(int numberOfPages)
    {
        // Arrange
        _request.NumberOfPages = numberOfPages;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.NumberOfPages);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void GivenNullEmptyOrWhitespacePrice_WhenValidate_ThenReturnError(decimal price)
    {
        // Arrange
        _request.Price = price;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void GivenValidRequest_WhenValidate_ThenReturnNoErrors()
    {
        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    public void GivenYearOfPublicationIsLowerOrEqualToZero_WhenValidate_ThenReturnError(int yearOfPublication)
    {
        // Arrange
        _request.YearOfPublication = yearOfPublication;

        // Act
        TestValidationResult<EditBookRequest>? validationResult = _validator.TestValidate(_request);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.YearOfPublication);
    }
}