using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.CoursAssistes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.CoursAssistes.GetReussiteParSessionPourProgramme;

public class
    GetReussiteParSessionPourProgrammeEndpoint(
        IMapper mapper,
        ICoursAssisteRepository coursAssisteRepository,
        IAuthenticatedMemberService authenticatedMemberService,
        ILogger<GetReussiteParSessionPourProgrammeEndpoint> logger)
    : Endpoint<GetReussiteParSessionPourProgrammeRequest,
        Dictionary<Guid, DictReussiteDto>>
{
    private readonly ILogger<GetReussiteParSessionPourProgrammeEndpoint> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Get("programmes/{id}/reussite");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetReussiteParSessionPourProgrammeRequest request, CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.Id))
        {
            await SendForbiddenAsync(ct);
        }

        Dictionary<Guid, Dictionary<Note, int>> dictReussiteParSessionEtude =
            coursAssisteRepository.GetReussiteParSessionPourProgramme(request.Id);

        Dictionary<Guid, DictReussiteDto> response = new();

        foreach ((Guid idSession, Dictionary<Note, int> dr) in dictReussiteParSessionEtude)
        {
            response[idSession] = new DictReussiteDto();
            foreach ((Note note, int nbEtudiantsAvecCetteNote) in dr)
            {
                response[idSession][note.ToString()] = nbEtudiantsAvecCetteNote;
            }
        }

        await SendAsync(response, cancellation: ct);
    }
}