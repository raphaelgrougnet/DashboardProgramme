using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Programmes.GetProgramme;

public class GetProgrammeValidator : Validator<GetProgrammeRequest>
{
    public GetProgrammeValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}