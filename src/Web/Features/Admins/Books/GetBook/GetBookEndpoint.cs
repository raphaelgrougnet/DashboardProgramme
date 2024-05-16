using Domain.Entities.Books;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Books.GetBook;

public class GetBookEndpoint : Endpoint<GetBookRequest, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookEndpoint(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("books/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetBookRequest request, CancellationToken ct)
    {
        Book book = _bookRepository.FindById(request.Id);
        await SendOkAsync(_mapper.Map<BookDto>(book), ct);
    }
}