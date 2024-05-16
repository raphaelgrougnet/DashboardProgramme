using Domain.Entities.Programmes;
using Domain.Repositories;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Programmes;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Admins.Programmes.GetProgramme;

public class GetProgrammeEndpoint : Endpoint<GetProgrammeRequest, ProgrammeDto>
{
    private readonly IMapper _mapper;
    private readonly IProgrammeRepository _programmeRepository;

    public GetProgrammeEndpoint(IMapper mapper, IProgrammeRepository programmeRepository)
    {
        _mapper = mapper;
        _programmeRepository = programmeRepository;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("programmes/{id}");
        Roles(Domain.Constants.User.Roles.ADMINISTRATOR);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(GetProgrammeRequest request, CancellationToken ct)
    {
        Programme programme = _programmeRepository.FindById(request.Id);
        await SendOkAsync(_mapper.Map<ProgrammeDto>(programme), ct);
    }
}