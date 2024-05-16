using Domain.Constants.User;
using Domain.Entities.Books;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Tests.Common.Builders;
using Tests.Common.Mapping;

using Web.Features.Admins.Books.GetBook;
using Web.Mapping.Profiles;

namespace Tests.Web.Features.Admins.Books.GetBook;

public class GetBookEndpointTests
{
    private readonly BookBuilder _bookBuilder;
    private readonly Mock<IBookRepository> _bookRepository;

    private readonly GetBookEndpoint _endPoint;

    public GetBookEndpointTests()
    {
        _bookBuilder = new BookBuilder();
        _bookRepository = new Mock<IBookRepository>();

        _endPoint = Factory.Create<GetBookEndpoint>(
            new MapperBuilder().WithProfile<ResponseMappingProfile>().Build(),
            _bookRepository.Object
        );
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
        _endPoint.Definition.Routes!.ShouldContain("books/{id}");
    }

    [Fact]
    public void WhenConfigure_ThenConfigureVerbToBeGet()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.Verbs!.ShouldContain(Http.GET.ToString());
    }

    [Fact]
    public async Task WhenHandleAsync_ThenDelegateGettingBookWithIdToBookRepository()
    {
        // Arrange
        Guid bookId = Guid.NewGuid();
        GetBookRequest request = new() { Id = bookId };

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _bookRepository.Verify(x => x.FindById(bookId));
    }

    [Fact]
    public async Task WhenHandleAsync_ThenReturnBookFoundByRepositoryAsBookDto()
    {
        // Arrange
        Book book = _bookBuilder.Build();
        _bookRepository.Setup(x => x.FindById(book.Id)).Returns(book);
        GetBookRequest request = new() { Id = book.Id };

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _endPoint.Response.Id.ShouldBe(book.Id);
    }

    [Fact]
    public async Task WhenHandleAsync_ThenReturnOkResult()
    {
        // Arrange
        Guid bookId = Guid.NewGuid();
        GetBookRequest request = new() { Id = bookId };

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _endPoint.HttpContext.Response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }
}