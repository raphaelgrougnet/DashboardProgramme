using Application.Interfaces.Services.Books;

using Domain.Constants.User;
using Domain.Entities.Books;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Tests.Common.Mapping;

using Web.Features.Admins.Books.CreateBook;
using Web.Mapping.Profiles;

namespace Tests.Web.Features.Admins.Books.EditBook;

public class EditBookEndpointTests
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

    private readonly Mock<IBookCreationService> _bookCreationService;

    private readonly CreateBookEndpoint _endPoint;

    public EditBookEndpointTests()
    {
        _bookCreationService = new Mock<IBookCreationService>();

        _endPoint = Factory.Create<CreateBookEndpoint>(
            new MapperBuilder().WithProfile<RequestMappingProfile>().Build(),
            _bookCreationService.Object
        );
    }

    private CreateBookRequest BuildValidRequest()
    {
        return new CreateBookRequest
        {
            NameFr = NAME_FR,
            NameEn = NAME_EN,
            DescriptionFr = DESCRIPTION_FR,
            DescriptionEn = DESCRIPTION_EN,
            Isbn = ISBN,
            Author = AUTHOR,
            Editor = EDITOR,
            YearOfPublication = YEAR_OF_PUBLICATION,
            NumberOfPages = NUMBER_OF_PAGES,
            Price = PRICE,
            CardImage = new FormFile(new MemoryStream(new byte[5]), long.MaxValue, long.MaxValue,
                "card image name", "any_filename")
        };
    }

    [Fact]
    public void WhenConfigure_ThenConfigureAllowedRoles()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.AllowedRoles!.ShouldContain(Roles.ADMINISTRATOR);
    }

    [Fact]
    public void WhenConfigure_ThenConfigureAuthSchemeToBeCookieAuthenticationScheme()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.AuthSchemeNames!.ShouldContain(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Fact]
    public void WhenConfigure_ThenConfigureRoute()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.Routes!.ShouldContain("books");
    }

    [Fact]
    public void WhenConfigure_ThenConfigureVerbToBePost()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.Verbs!.ShouldContain(Http.POST.ToString());
    }

    [Fact]
    public async Task WhenHandleAsync_ThenDelegateCreatingBookToBookUpdateService()
    {
        // Arrange
        CreateBookRequest request = BuildValidRequest();

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _bookCreationService.Verify(x => x.CreateBook(It.IsAny<Book>(), It.IsAny<IFormFile>()));
    }

    [Fact]
    public async Task WhenHandleAsync_ThenReturnOkResult()
    {
        // Arrange
        CreateBookRequest request = BuildValidRequest();

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _endPoint.HttpContext.Response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task WhenHandleAsync_ThenReturnSucceededOrNotResponseWithSucceededTrue()
    {
        // Arrange
        CreateBookRequest request = BuildValidRequest();

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _endPoint.Response.Succeeded.ShouldBeTrue();
    }
}