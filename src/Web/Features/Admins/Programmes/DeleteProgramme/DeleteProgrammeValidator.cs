using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Programmes.DeleteProgramme;

public class DeleteProgrammeValidator : Validator<DeleteProgrammeRequest>
{
    public DeleteProgrammeValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}