using Application.Interfaces.Services.Members;

using Domain.Entities;

using FastEndpoints;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Features.Admins.Members.DeleteMember;


public class DeleteMemberEndpoint(IAuthenticatedMemberService authenticatedMemberService, IMemberDeletionService memberDeletionService) : Endpoint<DeleteMemberRequest, EmptyResponse>
{
    public override void Configure()
    {
        DontCatchExceptions();
        
        Delete("admin/members/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        
    }

    public override async Task HandleAsync(DeleteMemberRequest request, CancellationToken ct)
    {
        Member myself = authenticatedMemberService.GetAuthenticatedMember();

        if (myself.Id == request.Id)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        await memberDeletionService.DeleteMemberWithId(request.Id);
        await SendNoContentAsync(ct);
    }
}