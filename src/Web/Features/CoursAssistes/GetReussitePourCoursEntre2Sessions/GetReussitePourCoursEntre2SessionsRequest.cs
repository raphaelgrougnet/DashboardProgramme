namespace Web.Features.CoursAssistes.GetReussitePourCoursEntre2Sessions;

public class GetReussitePourCoursEntre2SessionsRequest
{
    public Guid IdProgramme { get; set; }
    public Guid IdSessionDebut { get; set; }
    public Guid IdSessionFin { get; set; }
    public Guid IdCours { get; set; }
}