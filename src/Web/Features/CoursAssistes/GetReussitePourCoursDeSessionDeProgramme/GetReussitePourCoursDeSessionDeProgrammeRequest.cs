namespace Web.Features.CoursAssistes.GetReussitePourCoursDeSessionDeProgramme;

public class GetReussitePourCoursDeSessionDeProgrammeRequest
{
    public Guid IdProgramme { get; set; }
    public Guid IdSession { get; set; }
    public Guid IdCours { get; set; }
}