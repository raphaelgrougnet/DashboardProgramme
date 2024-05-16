using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.SessionEtudes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.SessionEtudes.GetSessionEtudesForProgramme;

public class GetSessionEtudesForProgrammeEndpoint : Endpoint<GetSessionEtudesForProgrammeRequest, List<SessionEtudeDto>>
{
    private readonly IAuthenticatedMemberService _authenticatedMemberService;
    private readonly IMapper _mapper;
    private readonly ISessionEtudeRepository _sessionEtudeRepository;

    public GetSessionEtudesForProgrammeEndpoint(IMapper mapper, IAuthenticatedMemberService authenticatedMemberService,
        ISessionEtudeRepository sessionEtudeRepository)
    {
        _mapper = mapper;
        _authenticatedMemberService = authenticatedMemberService;
        _sessionEtudeRepository = sessionEtudeRepository;
    }

    public override void Configure()
    {
        Get("programmes/{id}/sessionetudes");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetSessionEtudesForProgrammeRequest request, CancellationToken ct)
    {
        Member member = _authenticatedMemberService.GetAuthenticatedMember();
        IList<SessionEtude> lstSessionEtudes = _sessionEtudeRepository.GetAllForProgrammeId(request.Id);
        List<SessionEtudeDto> response = _mapper.Map<List<SessionEtudeDto>>(lstSessionEtudes);
        await SendAsync(response, cancellation: ct);
    }
}