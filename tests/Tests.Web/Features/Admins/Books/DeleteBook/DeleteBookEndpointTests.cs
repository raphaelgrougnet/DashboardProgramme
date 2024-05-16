using Domain.Constants.User;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Web.Features.Admins.Books.DeleteBook;

namespace Tests.Web.Features.Admins.Books.DeleteBook;

public class DeleteBookEndpointTests
{
    private readonly Mock<IBookRepository> _bookRepository;

    private readonly DeleteBookEndpoint _endPoint;

    public DeleteBookEndpointTests()
    {
        _bookRepository = new Mock<IBookRepository>();
        _endPoint = Factory.Create<DeleteBookEndpoint>(_bookRepository.Object);
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
    public void WhenConfigure_ThenConfigureVerbToBeDelete()
    {
        // Act
        _endPoint.Configure();

        // Assert
        _endPoint.Definition.Verbs!.ShouldContain(Http.DELETE.ToString());
    }

    [Fact]
    public async Task WhenHandleAsync_ThenDelegateCreatingBookToBookRepository()
    {
        // Arrange
        DeleteBookRequest request = new() { Id = Guid.NewGuid() };

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _bookRepository.Verify(x => x.DeleteBookWithId(request.Id));
    }

    [Fact]
    public async Task WhenHandleAsync_ThenReturnNoContentResult()
    {
        // Arrange
        DeleteBookRequest request = new() { Id = Guid.NewGuid() };

        // Act
        await _endPoint.HandleAsync(request, default);

        // Assert
        _endPoint.HttpContext.Response.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }
}