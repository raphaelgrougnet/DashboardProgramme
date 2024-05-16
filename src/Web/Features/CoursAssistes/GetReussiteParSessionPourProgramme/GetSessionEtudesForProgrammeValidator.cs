using FastEndpoints;

using FluentValidation;

namespace Web.Features.CoursAssistes.GetReussiteParSessionPourProgramme;

public class GetSessionEtudesForProgrammeValidator : Validator<GetReussiteParSessionPourProgrammeRequest>
{
    public GetSessionEtudesForProgrammeValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}