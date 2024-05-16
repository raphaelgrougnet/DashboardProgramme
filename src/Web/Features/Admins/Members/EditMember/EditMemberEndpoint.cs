using Application.Interfaces.Services.Members;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Members.EditMember;

public class EditMemberEndpoint : Endpoint<EditMemberRequest, SucceededOrNotResponse>
{
    private readonly IMapper _mapper;
    private readonly IMemberUpdateService _memberUpdateService;

    public EditMemberEndpoint(IMapper mapper, IMemberUpdateService memberUpdateService)
    {
        _mapper = mapper;
        _memberUpdateService = memberUpdateService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Patch("admin/members");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(EditMemberRequest req, CancellationToken ct)
    {
        string? role = req.Roles.Trim().ToLower() switch
        {
            Domain.Constants.User.Roles.ADMINISTRATOR => Domain.Constants.User.Roles.ADMINISTRATOR,
            _ => null
        };

        await _memberUpdateService.UpdateMember(req.Id, req.FirstName, req.LastName, req.Email, req.Password, role, req
            .Programmes);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}