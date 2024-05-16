namespace Web.Features.Etudiants.GetGradeAverage;

public class GetGradeAverageRequest
{
    public Guid IdProgramme { get; set; }
    public Guid IdSessionEtude { get; set; }
    public Guid IdCours { get; set; }
}