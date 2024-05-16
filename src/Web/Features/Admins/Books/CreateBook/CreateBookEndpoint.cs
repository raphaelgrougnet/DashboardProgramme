using Application.Interfaces.Services.Books;

using Domain.Entities.Books;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Books.CreateBook;

public class CreateBookEndpoint : Endpoint<CreateBookRequest, SucceededOrNotResponse>
{
    private readonly IBookCreationService _bookCreationService;
    private readonly IMapper _mapper;

    public CreateBookEndpoint(IMapper mapper, IBookCreationService bookCreationService)
    {
        _mapper = mapper;
        _bookCreationService = bookCreationService;
    }

    public override void Configure()
    {
        AllowFileUploads();
        DontCatchExceptions();

        Post("books");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        Book? book = _mapper.Map<Book>(req);
        await _bookCreationService.CreateBook(book, req.CardImage);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}