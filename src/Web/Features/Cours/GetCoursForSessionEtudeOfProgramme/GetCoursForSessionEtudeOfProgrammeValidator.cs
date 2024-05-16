using FastEndpoints;

using FluentValidation;

namespace Web.Features.Cours.GetCoursForSessionEtudeOfProgramme;

public class GetCoursForSessionEtudeOfProgrammeValidator : Validator<GetCoursForSessionEtudeOfProgrammeRequest>
{
    public GetCoursForSessionEtudeOfProgrammeValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
        RuleFor(x => x.IdSessionEtude)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionEtudeId")
            .WithMessage("SessionEtude id is required.");
    }
}