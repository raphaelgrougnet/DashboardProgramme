using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Members.GetMember;

public class GetMemberValidator : Validator<GetMemberRequest>
{
    public GetMemberValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyMemberId")
            .WithMessage("Member id is required.");
    }
}