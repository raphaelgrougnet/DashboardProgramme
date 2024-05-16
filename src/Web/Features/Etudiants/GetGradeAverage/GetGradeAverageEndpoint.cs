using Domain.Entities;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;
using Application.Interfaces.Services.Members;

using Domain.Entities.CoursAssistes;

using Web.Features.Cours.GetEtudiantsFromCours;

namespace Web.Features.Etudiants.GetGradeAverage;

public class GetGradeAverageEndpoint(IMapper mapper, 
    ICoursRepository coursRepository,
    IAuthenticatedMemberService authenticatedMemberService)
    : Endpoint<GetGradeAverageRequest, Dictionary<Note,int>>
{
    public override void Configure()
    {
        Get("programmes/{IdProgramme}/sessions/{IdSessionEtude}/cours/{IdCours}/notes");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    
    public override async Task HandleAsync(GetGradeAverageRequest request, CancellationToken ct)
    {
        Member member = authenticatedMemberService.GetAuthenticatedMember();
        if (!member.User.HasRole(Domain.Constants.User.Roles.ADMINISTRATOR) &&
            member.MemberProgrammes.All(mp => mp.Programme.Id != request.IdProgramme))
        {
            await SendForbiddenAsync(ct);
        }

        
        Dictionary<Note,int> countOfStudentsForGrades =
            coursRepository.GetAverageGradesForStudentsInClass(request.IdCours, request.IdSessionEtude, request.IdProgramme);
        await SendAsync(countOfStudentsForGrades, cancellation: ct);

    }
}