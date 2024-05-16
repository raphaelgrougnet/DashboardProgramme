using FastEndpoints;

using FluentValidation;

namespace Web.Features.Correlations.CalculatePValue;

public class CalculatePValueInternationalValidator : Validator<CalculatePValueInternationalRequest>
{
    public CalculatePValueInternationalValidator()
    {
        RuleFor(x => x.IdCours)
            .NotEmpty()
            .WithErrorCode("EmptyIdCours")
            .WithMessage("IdCours is required.");

        RuleFor(x => x.EtudiantInternational)
            .NotEmpty()
            .WithErrorCode("EmptyEtudiantInternational")
            .WithMessage("EtudiantInternational is required.");
    }
}