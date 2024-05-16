using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Members.DeleteMember;

public class DeleteMemberValidator : Validator<DeleteMemberRequest>
{
    public DeleteMemberValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyMemberId")
            .WithMessage("Member id is required.");
    }
}