using FastEndpoints;

using FluentValidation;

namespace Web.Features.Correlations.CalculatePValue;

public class CalculatePValueValidator : Validator<CalculatePValueRequest>
{
    public CalculatePValueValidator()
    {
        RuleFor(x => x.IdCours)
            .NotEmpty()
            .WithErrorCode("EmptyIdCours")
            .WithMessage("IdCours is required.");

        RuleFor(x => x.Critere)
            .NotEmpty()
            .WithErrorCode("EmptyCritere")
            .WithMessage("Critere is required.");
    }
}