using FastEndpoints;

using FluentValidation;

namespace Web.Features.Programmes.GetSpeAggregate;

public class GetSpeAggregateValidator : Validator<GetSpeAggregateRequest>
{
    public GetSpeAggregateValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}