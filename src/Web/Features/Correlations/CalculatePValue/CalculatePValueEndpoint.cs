using Application.Interfaces.Services.Correlations;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Correlations.CalculatePValue;

public class CalculatePValueEndpoint : Endpoint<CalculatePValueRequest, double>
{
    private readonly IDataCorrelationService _dataCorrelationService;
    private readonly IMapper _mapper;

    public CalculatePValueEndpoint(IMapper mapper, IDataCorrelationService dataCorrelationService)
    {
        _mapper = mapper;
        _dataCorrelationService = dataCorrelationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("correlations/calculate-p-value/{idCours}/{critere}");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CalculatePValueRequest req, CancellationToken ct)
    {
        double pValue;
        try
        {
            pValue = await _dataCorrelationService.CalculatePValue(req.IdCours, req.Critere);
        }
        catch (Exception)
        {
            await SendErrorsAsync(504, ct);
            return;
        }


        await SendOkAsync(pValue, ct);
    }
}