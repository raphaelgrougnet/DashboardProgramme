using FastEndpoints;

using FluentValidation;

namespace Web.Features.Correlations.CalculatePValueGENMELS;

public class CalculatePValueGENMELSValidator : Validator<CalculatePValueGENMELSRequest>
{
    public CalculatePValueGENMELSValidator()
    {
        RuleFor(x => x.IdCours)
            .NotEmpty()
            .WithErrorCode("EmptyIdCours")
            .WithMessage("IdCours is required.");
    }
}