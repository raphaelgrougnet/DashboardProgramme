using Application.Interfaces.Services.Members;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Members.CreateMember;

public class CreateMemberEndpoint : Endpoint<CreateMemberRequest, SucceededOrNotResponse>
{
    private readonly IMapper _mapper;
    private readonly IMemberCreationService _memberCreationService;

    public CreateMemberEndpoint(IMapper mapper, IMemberCreationService memberCreationService)
    {
        _mapper = mapper;
        _memberCreationService = memberCreationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Post("admin/members");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CreateMemberRequest req, CancellationToken ct)
    {
        string? role = req.Role.Trim().ToLower() switch
        {
            Domain.Constants.User.Roles.ADMINISTRATOR => Domain.Constants.User.Roles.ADMINISTRATOR,
            _ => null
        };

        await _memberCreationService.CreateMember(req.FirstName, req.LastName, req.Email, req.Password, role,
            req.Programmes);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}