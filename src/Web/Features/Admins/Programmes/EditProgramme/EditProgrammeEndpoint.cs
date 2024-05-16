using Application.Interfaces.Services.Programmes;

using Domain.Entities.Programmes;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Programmes.EditProgramme;

public class EditProgrammeEndpoint : Endpoint<EditProgrammeRequest, SucceededOrNotResponse>
{
    private readonly IMapper _mapper;
    private readonly IProgrammeUpdateService _programmeUpdateService;

    public EditProgrammeEndpoint(IMapper mapper, IProgrammeUpdateService programmeUpdateService)
    {
        _mapper = mapper;
        _programmeUpdateService = programmeUpdateService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Put("programmes/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(EditProgrammeRequest req, CancellationToken ct)
    {
        Programme? programme = _mapper.Map<Programme>(req);
        await _programmeUpdateService.UpdateProgramme(programme);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}