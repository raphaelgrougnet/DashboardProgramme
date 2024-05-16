using Domain.Entities.SessionEtudes;

namespace Domain.Repositories;

public interface ISessionEtudeRepository
{
    List<SessionEtude> GetAll();
    SessionEtude FindById(Guid id);
    IList<SessionEtude> GetAllForProgrammeId(Guid idProgramme);
    Task<SessionEtude> GetLatestForProgrammeId(Guid idProgramme);
    Task<SessionEtude> CreateSessionEtude(SessionEtude sessionEtude);

    SessionEtude? FindByAnneeAndSaison(ushort annee, Saison saison);
    Task SaveChangesAsync();
    Task<SessionEtude> AddSessionEtudeWithoutSaving(SessionEtude sessionEtude);
}