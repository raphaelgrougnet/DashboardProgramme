using FastEndpoints;

using FluentValidation;

namespace Web.Features.Correlations.CalculatePValueTourAdmission;

public class CalculatePValueTourAdmissionValidator : Validator<CalculatePValueTourAdmissionRequest>
{
    public CalculatePValueTourAdmissionValidator()
    {
        RuleFor(x => x.IdCours)
            .NotEmpty()
            .WithErrorCode("EmptyIdCours")
            .WithMessage("IdCours is required.");

        RuleFor(x => x.TourAdmission)
            .NotEmpty()
            .WithErrorCode("EmptyTourAdmission")
            .WithMessage("TourAdmission is required.");
    }
}