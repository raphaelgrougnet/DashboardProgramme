using Domain.Entities;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Members.GetAllMembers;

public class GetAllMembersEndpoint : EndpointWithoutRequest<List<MemberDto>>
{
    private readonly IMapper _mapper;
    private readonly IMemberRepository _memberRepository;

    public GetAllMembersEndpoint(IMapper mapper, IMemberRepository memberRepository)
    {
        _mapper = mapper;
        _memberRepository = memberRepository;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("admin/members");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        List<Member> members = _memberRepository.GetAll();
        await SendOkAsync(_mapper.Map<List<MemberDto>>(members), ct);
    }
}