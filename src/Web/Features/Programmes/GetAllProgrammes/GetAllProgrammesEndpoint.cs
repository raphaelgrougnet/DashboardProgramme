using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.Programmes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Programmes.GetAllProgrammes;

public class GetAllProgrammesEndpoint(
    IMapper mapper,
    IAuthenticatedMemberService authenticatedMemberService,
    IMemberProgrammeRepository memberProgrammeRepository,
    IProgrammeRepository programmeRepository)
    : EndpointWithoutRequest<List<ProgrammeDto>>
{
    public override void Configure()
    {
        Get("programmes");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        IList<Programme> lstProgrammes = member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR)
            ? programmeRepository.GetAll()
            : memberProgrammeRepository.GetProgrammesByMemberId(member.Id);
        List<ProgrammeDto> response = mapper.Map<List<ProgrammeDto>>(lstProgrammes);
        await SendAsync(response, cancellation: ct);
    }
}