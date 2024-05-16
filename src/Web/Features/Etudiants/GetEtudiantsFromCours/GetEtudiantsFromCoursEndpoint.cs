using Domain.Entities;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;
using Application.Interfaces.Services.Members;

using Web.Features.Cours.GetEtudiantsFromCours;

namespace Web.Features.Etudiants.GetEtudiantsFromCours;

public class GetEtudiantsFromCoursEndpoint(        
    IMapper mapper,
    ICoursRepository coursRepository,
    IAuthenticatedMemberService authenticatedMemberService)
    : Endpoint<GetEtudiantsFromCoursRequest, int>
{
    public override void Configure()
    {
        Get("programmes/{IdProgramme}/sessions/{IdSessionEtude}/cours/{IdCours}/etudiants");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetEtudiantsFromCoursRequest request, CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.IdProgramme))
        {
            await SendForbiddenAsync(ct);
        }

        // TODO : Ici not sure of EtudiantsDto ?
        
        int etudiantsCount =
                coursRepository.GetEtudiantsForCours(request.IdCours, request.IdSessionEtude, request.IdProgramme);
        await SendAsync(etudiantsCount, cancellation: ct);

    }

}