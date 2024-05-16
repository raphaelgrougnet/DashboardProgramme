using FastEndpoints;

using FluentValidation;

namespace Web.Features.CoursAssistes.GetReussitePourCoursDeSessionDeProgramme;

public class
    GetReussitePourCoursDeSessionDeProgrammeValidator : Validator<
    GetReussitePourCoursDeSessionDeProgrammeRequest>
{
    public GetReussitePourCoursDeSessionDeProgrammeValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
        RuleFor(x => x.IdSession)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionId")
            .WithMessage("Session id is required.");
        RuleFor(x => x.IdCours)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyCoursId")
            .WithMessage("Cours id is required.");
    }
}