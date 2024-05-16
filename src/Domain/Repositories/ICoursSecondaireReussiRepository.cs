using Domain.Entities.CoursSecondaireReussis;

namespace Domain.Repositories;

public interface ICoursSecondaireReussiRepository
{
    List<CoursSecondaireReussi> GetAll();
    CoursSecondaireReussi FindById(Guid id);
    Task CreateCoursSecondaireReussi(CoursSecondaireReussi cours);
    Task CreateCoursSecondaireReussiteWithoutSaveChanges(CoursSecondaireReussi cours);
    Task SaveChangesAsync();
    Task DeleteCoursSecondaireReussiWithId(Guid id);
    CoursSecondaireReussi FindByEtudiantAndCodeCourse(Guid etudiantId, string codeCours);
}