using FastEndpoints;

using FluentValidation;

using Web.Features.Cours.GetEtudiantsFromCours;

namespace Web.Features.Etudiants.GetGradeAverage;

public class GetGradeAverageValidator : Validator<GetGradeAverageRequest>
{
    public GetGradeAverageValidator()
    {
        RuleFor(x => x.IdSessionEtude)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionEtudeId")
            .WithMessage("SessionEtude id is required.");
        RuleFor(x => x.IdCours)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyCoursId")
            .WithMessage("Cours id is required.");
    }
}