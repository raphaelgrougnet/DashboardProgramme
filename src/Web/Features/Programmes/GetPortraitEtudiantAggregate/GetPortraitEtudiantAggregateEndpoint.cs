using Application.Interfaces.Services.SessionAssistees;

using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Programmes.GetPortraitEtudiantAggregate;

public class GetPortraitEtudiantAggregateEndpoint(
    IMapper mapper,
    ISessionEtudeRepository sessionEtudeRepository,
    IPortraitEtudiantQueryService portraitEtudiantQueryService)
    : Endpoint<GetPortraitEtudiantAggregateRequest, PortraitEtudiantAggregate>
{
    public override void Configure()
    {
        Get("programmes/{idProgramme}/portrait-etudiant");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetPortraitEtudiantAggregateRequest request, CancellationToken ct)
    {
        PortraitEtudiantAggregate portraitEtudiantAggregate =
            await portraitEtudiantQueryService.ObtenirPortraitEtudiantAggregate(request.IdProgramme);
        await SendAsync(portraitEtudiantAggregate, cancellation: ct);
    }
}