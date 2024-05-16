using Application.Interfaces.Services.SessionAssistees;

using Domain.Entities.SessionEtudes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Programmes.GetSpeAggregate;

public class GetSpeAggregateEndpoint(
    IMapper mapper,
    ISessionEtudeRepository sessionEtudeRepository,
    ISpeQueryService speQueryService)
    : Endpoint<GetSpeAggregateRequest, SpeAggregate>
{
    public override void Configure()
    {
        Get("programmes/{idProgramme}/spe");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetSpeAggregateRequest request, CancellationToken ct)
    {
        SessionEtude sessionActuelle = await sessionEtudeRepository.GetLatestForProgrammeId(request.IdProgramme);
        SpeAggregate speAggregate = await speQueryService.ObtenirSpeAggregate(sessionActuelle.Id, request.IdProgramme);
        await SendAsync(speAggregate, cancellation: ct);
    }
}