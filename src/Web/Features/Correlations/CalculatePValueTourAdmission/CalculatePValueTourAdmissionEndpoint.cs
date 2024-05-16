using Application.Interfaces.Services.Correlations;

using FastEndpoints;

using Microsoft.AspNetCore.Authentication.Cookies;

using IMapper = AutoMapper.IMapper;

namespace Web.Features.Correlations.CalculatePValueTourAdmission;

public class CalculatePValueTourAdmissionEndpoint : Endpoint<CalculatePValueTourAdmissionRequest, double>
{
    private readonly IDataCorrelationService _dataCorrelationService;
    private readonly IMapper _mapper;

    public CalculatePValueTourAdmissionEndpoint(IMapper mapper, IDataCorrelationService dataCorrelationService)
    {
        _mapper = mapper;
        _dataCorrelationService = dataCorrelationService;
    }

    public override void Configure()
    {
        DontCatchExceptions();

        Get("correlations/calculate-p-value-tour-admission/{idCours}/{tourAdmission}");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CalculatePValueTourAdmissionRequest req, CancellationToken ct)
    {
        double pValue;
        try
        {
            pValue = await _dataCorrelationService.CalculatePValueTourAdmission(req.IdCours, req.TourAdmission);
        }
        catch (Exception)
        {
            await SendErrorsAsync(504, ct);
            return;
        }


        await SendOkAsync(pValue, ct);
    }
}