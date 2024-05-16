using Application.Interfaces.Services.Correlations;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Correlations.CalculatePValueGENMELS;

public class CalculatePValueGENMELSEndpoint : Endpoint<CalculatePValueGENMELSRequest, double>
{
    private readonly IDataCorrelationService _dataCorrelationService;
    private readonly IMapper _mapper;

    public CalculatePValueGENMELSEndpoint(IMapper mapper, IDataCorrelationService dataCorrelationService)
    {
        _mapper = mapper;
        _dataCorrelationService = dataCorrelationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("correlations/calculate-p-value-genmels/{idCours}");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CalculatePValueGENMELSRequest req, CancellationToken ct)
    {
        double pValue;
        try
        {
            pValue = await _dataCorrelationService.CalculatePValueGENMELS(req.IdCours);
        }
        catch (Exception)
        {
            await SendErrorsAsync(504, ct);
            return;
        }


        await SendOkAsync(pValue, ct);
    }
}