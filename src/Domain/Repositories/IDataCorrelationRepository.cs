namespace Domain.Repositories;

public interface IDataCorrelationRepository
{
    IEnumerable<(float noteMoyenne, IEnumerable<string> coursSecondaire)>
        GetNoteWithCoursSecondaireReussisForGivenCoursId(Guid id);

    IEnumerable<(float noteMoyenne, float GENMELS)> GetNoteWithGENMELSForGivenCoursId(Guid id);

    IEnumerable<(float noteMoyenne, byte tourAdmission)> GetNoteWithWithTourAdmissionForGivenCoursId(Guid id);

    IEnumerable<(float noteMoyenne, string etudiantInternational)> GetNoteWithWithInternationalForGivenCoursId(Guid id);
}