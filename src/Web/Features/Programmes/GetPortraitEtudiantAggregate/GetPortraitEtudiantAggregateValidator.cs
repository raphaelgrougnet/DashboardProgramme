using FastEndpoints;

using FluentValidation;

namespace Web.Features.Programmes.GetPortraitEtudiantAggregate;

public class GetPortraitEtudiantAggregateValidator : Validator<GetPortraitEtudiantAggregateRequest>
{
    public GetPortraitEtudiantAggregateValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
    }
}