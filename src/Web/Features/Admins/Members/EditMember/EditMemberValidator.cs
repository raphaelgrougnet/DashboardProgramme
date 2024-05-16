using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Members.EditMember;

public class EditMemberValidator : Validator<EditMemberRequest>
{
    public EditMemberValidator()
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

        RuleFor(x => x.Password);

        RuleFor(x => x.Roles);

        RuleFor(x => x.Programmes)
            .NotNull()
            .WithErrorCode("InvalidProgrammes")
            .WithMessage("Member programmes should not be null.");
    }
}