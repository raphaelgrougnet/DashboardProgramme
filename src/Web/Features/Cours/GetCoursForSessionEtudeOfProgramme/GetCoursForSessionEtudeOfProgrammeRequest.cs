namespace Web.Features.Cours.GetCoursForSessionEtudeOfProgramme;

public class GetCoursForSessionEtudeOfProgrammeRequest
{
    public Guid IdProgramme { get; set; }
    public Guid IdSessionEtude { get; set; }
}