using FastEndpoints;

using FluentValidation;

namespace Web.Features.Cours.GetCoursFromCode;

public class GetCoursFromCodeValidator : Validator<GetCoursFromCodeRequest>
{
    public GetCoursFromCodeValidator()
    {
        RuleFor(x => x.Code)
            .NotEqual(string.Empty)
            .WithErrorCode("EmptyCoursCode")
            .WithMessage("Cours code is required.");
    }
}