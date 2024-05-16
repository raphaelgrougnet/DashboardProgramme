using Domain.Entities.CoursAssistes;

namespace Domain.Repositories;

public interface ICoursAssisteRepository
{
    List<CoursAssiste> GetAll();
    CoursAssiste FindById(Guid id);
    List<CoursAssiste> FindBySessionAssisteeId(Guid id);

    Dictionary<Note, int> GetReussitePourCoursDeSessionDeProgramme(Guid idProgramme, Guid
        idSession, Guid idCours);

    Dictionary<Note, int> GetReussitePourCoursEntre2Sessions(Guid idProgramme, Guid
        idCours, Guid idSessonDebut, Guid idSessionFin);

    Task CreateCoursAssiste(CoursAssiste coursAssiste);
    Task<CoursAssiste> CreateCoursAssisteWithoutSaveChanges(CoursAssiste coursAssiste);
    Task SaveChangesAsync();
    Dictionary<Guid, Dictionary<Note, int>> GetReussiteParSessionPourProgramme(Guid idProgramme);

    Dictionary<Guid, Dictionary<Note, int>> GetReussiteParCoursPourProgrammeEtSession(Guid idProgramme,
        Guid idSessionEtude);

    CoursAssiste? FindBySessionAssisteeIdAndCoursId(Guid idSessionAssistee, Guid idCours);
}