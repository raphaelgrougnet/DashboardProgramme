using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Programmes.CreateProgramme;

public class CreateProgrammeValidator : Validator<CreateProgrammeRequest>
{
    public CreateProgrammeValidator()
    {
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