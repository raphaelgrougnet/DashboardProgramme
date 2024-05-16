namespace Web.Features.Cours.GetEtudiantsFromCours;

public class GetEtudiantsFromCoursRequest
{
    public Guid IdProgramme { get; set; }
    public Guid IdSessionEtude { get; set; }
    public Guid IdCours { get; set; }
}