using Domain.Entities;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Members;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Members.GetMember;

public class GetMemberEndpoint : Endpoint<GetMemberRequest, MemberDto>
{
    private readonly IMapper _mapper;
    private readonly IMemberRepository _memberRepository;

    public GetMemberEndpoint(IMapper mapper, IMemberRepository memberRepository)
    {
        _mapper = mapper;
        _memberRepository = memberRepository;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("admin/members/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetMemberRequest request, CancellationToken ct)
    {
        Member member = _memberRepository.FindById(request.Id);
        await SendOkAsync(_mapper.Map<MemberDto>(member), ct);
    }
}