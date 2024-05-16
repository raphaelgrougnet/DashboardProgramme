using Application.Interfaces.Services.Members;

using Domain.Entities;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Members.Me.GetMe;

public class GetMeEndpoint : EndpointWithoutRequest<GetMeResponse>
{
    private readonly IAuthenticatedMemberService _authenticatedMemberService;
    private readonly IMapper _mapper;

    public GetMeEndpoint(IMapper mapper, IAuthenticatedMemberService authenticatedMemberService)
    {
        _mapper = mapper;
        _authenticatedMemberService = authenticatedMemberService;
    }

    public override void Configure()
    {
        Get("members/me");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Member member = _authenticatedMemberService.GetAuthenticatedMember();
        GetMeResponse? response = _mapper.Map<GetMeResponse>(member);
        await SendAsync(response, cancellation: ct);
    }
}