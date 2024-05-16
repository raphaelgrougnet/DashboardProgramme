using Application.Interfaces.Services.Correlations;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using Web.Features.Correlations.CalculatePValue;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Correlations.CalculatePValueInternational;

public class CalculatePValueInternationalEndpoint : Endpoint<CalculatePValueInternationalRequest, double>
{
    private readonly IDataCorrelationService _dataCorrelationService;
    private readonly IMapper _mapper;

    public CalculatePValueInternationalEndpoint(IMapper mapper, IDataCorrelationService dataCorrelationService)
    {
        _mapper = mapper;
        _dataCorrelationService = dataCorrelationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("correlations/calculate-p-value-international/{idCours}/{etudiantInternational}");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CalculatePValueInternationalRequest req, CancellationToken ct)
    {
        double pValue;
        try
        {
            pValue = await _dataCorrelationService.CalculatePValueInternational(req.IdCours, req.EtudiantInternational);
        }
        catch (Exception)
        {
            await SendErrorsAsync(504, ct);
            return;
        }


        await SendOkAsync(pValue, ct);
    }
}