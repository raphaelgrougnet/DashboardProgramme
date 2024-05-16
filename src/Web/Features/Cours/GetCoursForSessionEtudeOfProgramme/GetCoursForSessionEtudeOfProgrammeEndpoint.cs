using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Cours.GetCoursForSessionEtudeOfProgramme;

public class GetCoursForSessionEtudeOfProgrammeEndpoint(
        IMapper mapper,
        ICoursRepository coursRepository,
        IAuthenticatedMemberService authenticatedMemberService)
    : Endpoint<GetCoursForSessionEtudeOfProgrammeRequest, List<CoursDto>>
{
    public override void Configure()
    {
        Get("programmes/{IdProgramme}/sessions/{IdSessionEtude}/cours");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetCoursForSessionEtudeOfProgrammeRequest request, CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.IdProgramme))
        {
            await SendForbiddenAsync(ct);
        }

        IList<Domain.Entities.CoursNs.Cours> lstCours =
            coursRepository.GetCoursForSessionEtudeOfProgramme(request.IdProgramme, request.IdSessionEtude);
        List<CoursDto> response = mapper.Map<List<CoursDto>>(lstCours);
        await SendAsync(response, cancellation: ct);
    }
}