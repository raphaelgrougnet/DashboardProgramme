using FastEndpoints;

using FluentValidation;

namespace Web.Features.CoursAssistes.GetReussitePourCoursEntre2Sessions;

public class GetReussitePourCoursEntre2SessionsValidator : Validator<GetReussitePourCoursEntre2SessionsRequest>
{
    public GetReussitePourCoursEntre2SessionsValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
        RuleFor(x => x.IdSessionDebut)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionDebutId")
            .WithMessage("Session debut id is required.");
        RuleFor(x => x.IdSessionFin)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionFinId")
            .WithMessage("Session fin id is required.");
        RuleFor(x => x.IdCours)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyCoursId")
            .WithMessage("Cours id is required.");
    }
}