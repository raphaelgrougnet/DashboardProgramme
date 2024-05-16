namespace Web.Features.Cours.GetEtudiantsFromCours;

using FastEndpoints;

using FluentValidation;


public class GetEtudiantsFromCoursValidator : Validator<GetEtudiantsFromCoursRequest>
{
    public GetEtudiantsFromCoursValidator()
    {
        RuleFor(x => x.IdProgramme)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyProgrammeId")
            .WithMessage("Programme id is required.");
        RuleFor(x => x.IdSessionEtude)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptySessionEtudeId")
            .WithMessage("SessionEtude id is required.");
        RuleFor(x => x.IdCours)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyCoursId")
            .WithMessage("Cours id is required.");
        
    }
}