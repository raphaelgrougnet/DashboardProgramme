using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Members.CreateMember;

public class CreateMemberValidator : Validator<CreateMemberRequest>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidFirstName")
            .WithMessage("Member first name should not be empty or null.");

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidLastName")
            .WithMessage("Member last name should not be empty or null.");

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode("InvalidEmail")
            .WithMessage("Member email should not be empty or null and should be a valid email address.");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidPassword")
            .WithMessage("Member password should not be empty or null.");

        RuleFor(x => x.Role)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("InvalidRole")
            .WithMessage("Member role should not be empty or null.");
    }
}