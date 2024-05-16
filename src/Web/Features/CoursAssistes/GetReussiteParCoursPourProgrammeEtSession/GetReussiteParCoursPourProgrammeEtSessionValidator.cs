using FastEndpoints;

using FluentValidation;

namespace Web.Features.CoursAssistes.GetReussiteParCoursPourProgrammeEtSession;

public class
    GetReussiteParCoursPourProgrammeEtSessionValidator : Validator<GetReussiteParCoursPourProgrammeEtSessionRequest>
{
    public GetReussiteParCoursPourProgrammeEtSessionValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");

        RuleFor(x => x.IdSessionEtude)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionId")
            .WithMessage("SessionEtude id is required.");
    }
}