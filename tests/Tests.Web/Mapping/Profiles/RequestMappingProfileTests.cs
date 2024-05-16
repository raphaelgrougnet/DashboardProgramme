using AutoMapper;

using Domain.Common;
using Domain.Entities.Books;

using Tests.Common.Mapping;
using Tests.Web.TestCollections;

using Web.Dtos;
using Web.Features.Admins.Books.CreateBook;
using Web.Features.Admins.Books.EditBook;
using Web.Mapping.Profiles;

namespace Tests.Web.Mapping.Profiles;

[Collection(WebTestCollection.NAME)]
public class RequestMappingProfileTests
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

    private readonly IMapper _mapper = new MapperBuilder()
        .WithProfile<RequestMappingProfile>()
        .Build();

    [Fact]
    public void GivenCreateBookRequest_WhenMap_ThenReturnBookMappedCorrectly()
    {
        // Arrange
        CreateBookRequest createBookRequest = new()
        {
            NameFr = NAME_FR,
            NameEn = NAME_EN,
            Price = PRICE,
            DescriptionFr = DESCRIPTION_FR,
            DescriptionEn = DESCRIPTION_EN,
            Isbn = ISBN,
            Author = AUTHOR,
            Editor = EDITOR,
            YearOfPublication = YEAR_OF_PUBLICATION,
            NumberOfPages = NUMBER_OF_PAGES
        };

        // Act
        Book? book = _mapper.Map<Book>(createBookRequest);

        // Assert
        book.NameFr.ShouldBe(NAME_FR);
        book.NameEn.ShouldBe(NAME_EN);
        book.DescriptionFr.ShouldBe(DESCRIPTION_FR);
        book.DescriptionEn.ShouldBe(DESCRIPTION_EN);
        book.Isbn.ShouldBe(ISBN);
        book.Author.ShouldBe(AUTHOR);
        book.Editor.ShouldBe(EDITOR);
        book.YearOfPublication.ShouldBe(YEAR_OF_PUBLICATION);
        book.NumberOfPages.ShouldBe(NUMBER_OF_PAGES);
    }

    [Fact]
    public void GivenEditBookRequest_WhenMap_ThenReturnBookMappedCorrectly()
    {
        // Arrange
        Guid bookId = Guid.NewGuid();
        EditBookRequest editBookRequest = new()
        {
            Id = bookId,
            NameFr = NAME_FR,
            NameEn = NAME_EN,
            Price = PRICE,
            DescriptionFr = DESCRIPTION_FR,
            DescriptionEn = DESCRIPTION_EN,
            Isbn = ISBN,
            Author = AUTHOR,
            Editor = EDITOR,
            YearOfPublication = YEAR_OF_PUBLICATION,
            NumberOfPages = NUMBER_OF_PAGES
        };

        // Act
        Book? book = _mapper.Map<Book>(editBookRequest);

        // Assert
        book.Id.ShouldBe(bookId);
        book.NameFr.ShouldBe(NAME_FR);
        book.NameEn.ShouldBe(NAME_EN);
        book.DescriptionFr.ShouldBe(DESCRIPTION_FR);
        book.DescriptionEn.ShouldBe(DESCRIPTION_EN);
        book.Isbn.ShouldBe(ISBN);
        book.Author.ShouldBe(AUTHOR);
        book.Editor.ShouldBe(EDITOR);
        book.YearOfPublication.ShouldBe(YEAR_OF_PUBLICATION);
        book.NumberOfPages.ShouldBe(NUMBER_OF_PAGES);
    }

    [Fact]
    public void GivenTranslatableString_WhenMap_ThenReturnTranslatableStringDtoMappedCorrectly()
    {
        // Arrange
        TranslatableString translatableString = new() { Fr = NAME_FR, En = NAME_EN };

        // Act
        TranslatableStringDto? translatableStringDto = _mapper.Map<TranslatableStringDto>(translatableString);

        // Assert
        translatableStringDto.Fr.ShouldBe(NAME_FR);
        translatableStringDto.En.ShouldBe(NAME_EN);
    }

    [Fact]
    public void GivenTranslatableStringDto_WhenMap_ThenReturnTranslatableStringMappedCorrectly()
    {
        // Arrange
        TranslatableStringDto translatableStringDto = new() { Fr = NAME_FR, En = NAME_EN };

        // Act
        TranslatableString? translatableString = _mapper.Map<TranslatableString>(translatableStringDto);

        // Assert
        translatableString.Fr.ShouldBe(NAME_FR);
        translatableString.En.ShouldBe(NAME_EN);
    }
}