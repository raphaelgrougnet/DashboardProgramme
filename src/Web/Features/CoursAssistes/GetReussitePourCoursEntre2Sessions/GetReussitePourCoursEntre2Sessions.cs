using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.CoursAssistes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.CoursAssistes.GetReussitePourCoursEntre2Sessions;

public class GetReussitePourCoursEntre2SessionsEndpoint(
    IMapper mapper,
    IAuthenticatedMemberService authenticatedMemberService,
    ICoursAssisteRepository coursAssisteRepository)
    :
        Endpoint<GetReussitePourCoursEntre2SessionsRequest, DictReussiteDto>
{
    private readonly IMapper _mapper = mapper;

    public override void Configure()
    {
        Get("programmes/{idProgramme}/comparerEntre/{idSessionDebut}/{idSessionFin}/cours/{idCours}/reussite");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetReussitePourCoursEntre2SessionsRequest request,
        CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.IdProgramme))
        {
            await SendForbiddenAsync(ct);
        }

        Dictionary<Note, int> totalEtudiants =
            coursAssisteRepository.GetReussitePourCoursEntre2Sessions(request
                .IdProgramme, request.IdCours, request.IdSessionDebut, request.IdSessionFin);

        DictReussiteDto response = new();

        foreach ((Note note, int nbEtudiantsAvecCetteNote) in totalEtudiants)
        {
            response[note.ToString()] = nbEtudiantsAvecCetteNote;
        }

        await SendOkAsync(response, ct);
    }
}