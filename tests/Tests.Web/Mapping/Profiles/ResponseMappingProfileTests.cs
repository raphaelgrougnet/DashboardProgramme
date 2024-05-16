using Application.Common;

using AutoMapper;

using Domain.Entities.Books;

using Microsoft.AspNetCore.Identity;

using Tests.Common.Builders;
using Tests.Common.Mapping;
using Tests.Web.TestCollections;

using Web.Constants;
using Web.Features.Admins.Books;
using Web.Features.Common;
using Web.Mapping.Profiles;

namespace Tests.Web.Mapping.Profiles;

[Collection(WebTestCollection.NAME)]
public class ResponseMappingProfileTests
{
    private const string ANY_ERROR_DESCRIPTION = "Could not create user.";

    private const string NAME_FR = "Guide de la réglementation en copropriété";
    private const string NAME_EN = "Guide to condominium regulations";
    private const string DESCRIPTION_FR = "On vise, en copropriété, à assurer aux occupant un milieu de vie paisible.";

    private const string DESCRIPTION_EN =
        "We aim, in co-ownership, to provide occupants with a peaceful living environment.";

    private const string ISBN = "978-2-89689-559-5";
    private const string AUTHOR = "Christine Gagnon, Yves Papineau";
    private const string EDITOR = "Wilson & Lafleur";
    private const string CARD_IMAGE = "www.google.com";
    private const int YEAR_OF_PUBLICATION = 2023;
    private const int NUMBER_OF_PAGES = 346;

    private readonly BookBuilder _bookBuilder = new();

    private readonly IMapper _mapper = new MapperBuilder().WithProfile<ResponseMappingProfile>().Build();

    [Fact]
    public void GivenBook_WhenMap_ThenReturnBookDtoMappedCorrectly()
    {
        // Arrange
        Book book = _bookBuilder
            .WithName(NAME_FR, NAME_EN)
            .WithDescriptions(DESCRIPTION_FR, DESCRIPTION_EN)
            .WithIsbn(ISBN)
            .WithAuthor(AUTHOR)
            .WithEditor(EDITOR)
            .WithCardImage(CARD_IMAGE)
            .WithYearOfPublication(YEAR_OF_PUBLICATION)
            .WithNumberOfPages(NUMBER_OF_PAGES)
            .Build();

        // Act
        BookDto? bookDto = _mapper.Map<BookDto>(book);

        // Assert
        bookDto.Id.ShouldBe(book.Id);
        bookDto.NameFr.ShouldBe(NAME_FR);
        bookDto.NameEn.ShouldBe(NAME_EN);
        bookDto.DescriptionFr.ShouldBe(DESCRIPTION_FR);
        bookDto.DescriptionEn.ShouldBe(DESCRIPTION_EN);
        bookDto.Isbn.ShouldBe(ISBN);
        bookDto.Author.ShouldBe(AUTHOR);
        bookDto.Editor.ShouldBe(EDITOR);
        bookDto.CardImage.ShouldBe(CARD_IMAGE);
        bookDto.YearOfPublication.ShouldBe(YEAR_OF_PUBLICATION);
        bookDto.NumberOfPages.ShouldBe(NUMBER_OF_PAGES);
    }

    [Fact]
    public void GivenFailedIdentityResult_WhenMap_ThenSucceededContainsErrorWithIdentityCodeAsErrorType()
    {
        // Arrange
        IdentityError error = new() { Code = IdentityResultExceptions.USER_ALREADY_HAS_PASSWORD };
        IdentityResult? identityResult = IdentityResult.Failed(error);

        // Act
        SucceededOrNotResponse? succeededOrNotResponse = _mapper.Map<SucceededOrNotResponse>(identityResult);

        // Assert
        succeededOrNotResponse.Errors.ShouldContain(x => x.ErrorType == error.Code);
    }

    [Fact]
    public void GivenFailedIdentityResult_WhenMap_ThenSucceededContainsErrorWithIdentityErrorDescriptionAsErrorMessage()
    {
        // Arrange
        IdentityError error = new() { Description = ANY_ERROR_DESCRIPTION };
        IdentityResult? identityResult = IdentityResult.Failed(error);

        // Act
        SucceededOrNotResponse? succeededOrNotResponse = _mapper.Map<SucceededOrNotResponse>(identityResult);

        // Assert
        succeededOrNotResponse.Errors.ShouldContain(x => x.ErrorMessage == ANY_ERROR_DESCRIPTION);
    }

    [Fact]
    public void GivenIdentityErrorWithCode_WhenMap_ThenErrorHasIdentityResultCodeAsErrorType()
    {
        // Arrange
        IdentityError identityError = new() { Code = IdentityResultExceptions.USER_ALREADY_HAS_PASSWORD };

        // Act
        Error? error = _mapper.Map<Error>(identityError);

        // Assert
        error.ErrorType.ShouldBe(IdentityResultExceptions.USER_ALREADY_HAS_PASSWORD);
    }

    [Fact]
    public void GivenIdentityErrorWithCode_WhenMap_ThenErrorHasIdentityResultDescriptionAsErrorMessage()
    {
        // Arrange
        IdentityError identityError = new() { Description = ANY_ERROR_DESCRIPTION };

        // Act
        Error? error = _mapper.Map<Error>(identityError);

        // Assert
        error.ErrorMessage.ShouldBe(ANY_ERROR_DESCRIPTION);
    }

    [Fact]
    public void GivenSuccessfulIdentityResult_WhenMap_ThenSucceededIsTrue()
    {
        // Arrange
        IdentityResult? identityResult = IdentityResult.Success;

        // Act
        SucceededOrNotResponse? succeededOrNotResponse = _mapper.Map<SucceededOrNotResponse>(identityResult);

        // Assert
        succeededOrNotResponse.Succeeded.ShouldBeTrue();
    }
}