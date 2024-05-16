using FastEndpoints;

using FluentValidation;

namespace Web.Features.SessionEtudes.GetSessionEtudesForProgramme;

public class GetSessionEtudesForProgrammeValidator : Validator<GetSessionEtudesForProgrammeRequest>
{
    public GetSessionEtudesForProgrammeValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}