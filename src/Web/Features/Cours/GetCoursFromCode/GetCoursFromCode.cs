using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Cours.GetCoursFromCode;

public class GetCoursFromCodeEndpoint(IMapper mapper, ICoursRepository coursRepository)
    : Endpoint<GetCoursFromCodeRequest, CoursDto>
{
    public override void Configure()
    {
        DontCatchExceptions();

        Get("cours/{code}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetCoursFromCodeRequest request, CancellationToken ct)
    {
        // Utiliser « Cours » crée une confusion entre la classe et le namespace
        Domain.Entities.CoursNs.Cours cours = coursRepository.FindByCode(request.Code);
        await SendOkAsync(mapper.Map<CoursDto>(cours), ct);
    }
}