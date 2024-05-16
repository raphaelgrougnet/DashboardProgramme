using Application.Interfaces.Services.Books;

using Domain.Entities.Books;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Books.EditBook;

public class EditBookEndpoint : Endpoint<EditBookRequest, SucceededOrNotResponse>
{
    private readonly IBookUpdateService _bookUpdateService;
    private readonly IMapper _mapper;

    public EditBookEndpoint(IMapper mapper, IBookUpdateService bookUpdateService)
    {
        _mapper = mapper;
        _bookUpdateService = bookUpdateService;
    }

    public override void Configure()
    {
        AllowFileUploads();
        DontCatchExceptions();

        Put("books/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(EditBookRequest req, CancellationToken ct)
    {
        Book? book = _mapper.Map<Book>(req);
        await _bookUpdateService.UpdateBook(book, req.CardImage);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}