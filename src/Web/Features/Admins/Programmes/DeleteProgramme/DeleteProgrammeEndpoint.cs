using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Features.Admins.Programmes.DeleteProgramme;

public class DeleteProgrammeEndpoint : Endpoint<DeleteProgrammeRequest, EmptyResponse>
{
    private readonly IProgrammeRepository _programmeRepository;

    public DeleteProgrammeEndpoint(IProgrammeRepository programmeRepository)
    {
        _programmeRepository = programmeRepository;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Delete("programmes/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(DeleteProgrammeRequest request, CancellationToken ct)
    {
        await _programmeRepository.DeleteProgrammeWithId(request.Id);
        await SendNoContentAsync(ct);
    }
}