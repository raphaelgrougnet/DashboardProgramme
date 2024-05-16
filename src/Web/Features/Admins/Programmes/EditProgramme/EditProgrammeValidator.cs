using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Programmes.EditProgramme;

public class EditProgrammeValidator : Validator<EditProgrammeRequest>
{
    public EditProgrammeValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("emptyProgrammeId")
            .WithMessage("Programme id is required.");

        RuleFor(x => x.Nom)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidNom")
            .WithMessage("Nom programme should not be empty or null.");

        RuleFor(x => x.Numero)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidNumero")
            .WithMessage("Numero programme should not be empty or null.");
    }
}