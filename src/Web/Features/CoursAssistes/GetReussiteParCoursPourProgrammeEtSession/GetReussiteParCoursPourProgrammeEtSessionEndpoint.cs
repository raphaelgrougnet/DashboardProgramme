using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.CoursAssistes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.CoursAssistes.GetReussiteParCoursPourProgrammeEtSession;

public class
    GetReussiteParCoursPourProgrammeEtSessionEndpoint(
        IMapper mapper,
        ICoursAssisteRepository coursAssisteRepository,
        IAuthenticatedMemberService authenticatedMemberService,
        ILogger<GetReussiteParCoursPourProgrammeEtSessionEndpoint> logger)
    : Endpoint<GetReussiteParCoursPourProgrammeEtSessionRequest,
        Dictionary<Guid, DictReussiteDto>>
{
    private readonly ILogger<GetReussiteParCoursPourProgrammeEtSessionEndpoint> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Get("programmes/{IdProgramme}/sessions/{IdSessionEtude}/reussite");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetReussiteParCoursPourProgrammeEtSessionRequest request,
        CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.IdProgramme))
        {
            await SendForbiddenAsync(ct);
        }

        Dictionary<Guid, Dictionary<Note, int>> dictReussiteParCours =
            coursAssisteRepository.GetReussiteParCoursPourProgrammeEtSession(request.IdProgramme,
                request.IdSessionEtude);

        Dictionary<Guid, DictReussiteDto> response = new();

        foreach ((Guid idCours, Dictionary<Note, int> dr) in dictReussiteParCours)
        {
            response[idCours] = new DictReussiteDto();
            foreach ((Note note, int nbEtudiantsAvecCetteNote) in dr)
            {
                response[idCours][note.ToString()] = nbEtudiantsAvecCetteNote;
            }
        }

        await SendAsync(response, cancellation: ct);
    }
}