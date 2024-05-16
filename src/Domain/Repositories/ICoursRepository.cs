using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.Etudiants;

namespace Domain.Repositories;

public interface ICoursRepository
{
    List<Cours> GetAll();
    List<Cours> GetCoursForSessionEtudeOfProgramme(Guid idProgramme, Guid idSessionEtude);
    int GetEtudiantsForCours(Guid coursId, Guid idSessionEtude, Guid idProgramme);
    Dictionary<Note,int> GetAverageGradesForStudentsInClass(Guid coursId, Guid idSessoinEtude, Guid idProgramme);
    Cours FindById(Guid id);
    Cours? FindByCode(string code);
    Task<Cours> CreateCours(Cours cours);
    Task<Cours> AddCoursWithoutSaveChanges(Cours cours);
    Task SaveChangesAsync();
}