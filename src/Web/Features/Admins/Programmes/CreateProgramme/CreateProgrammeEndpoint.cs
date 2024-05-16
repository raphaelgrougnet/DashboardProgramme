using Application.Interfaces.Services.Programmes;

using Domain.Entities.Programmes;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Common;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Programmes.CreateProgramme;

public class CreateProgrammeEndpoint : Endpoint<CreateProgrammeRequest, SucceededOrNotResponse>
{
    private readonly IMapper _mapper;
    private readonly IProgrammeCreationService _programmeCreationService;

    public CreateProgrammeEndpoint(IMapper mapper, IProgrammeCreationService programmeCreationService)
    {
        _mapper = mapper;
        _programmeCreationService = programmeCreationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Post("programmes");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CreateProgrammeRequest req, CancellationToken ct)
    {
        Programme? programme = _mapper.Map<Programme>(req);
        await _programmeCreationService.CreateProgramme(programme);
        await SendOkAsync(new SucceededOrNotResponse(true), ct);
    }
}