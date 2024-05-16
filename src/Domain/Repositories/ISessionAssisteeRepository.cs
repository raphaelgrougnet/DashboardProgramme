using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;

namespace Domain.Repositories;

public interface ISessionAssisteeRepository
{
    List<SessionAssistee> GetAll();
    SessionAssistee FindById(Guid id);
    Task<SessionAssistee> AddSessionAssisteeWithoutSaving(SessionAssistee sessionAssistee);
    Task SaveChangesAsync();

    SessionAssistee? FindByParams(Etudiant etudiant, GrilleProgramme grilleProgramme, SessionEtude se, ushort nbHeures,
        byte nIemeSession,
        bool estBeneficiaireServicesAdaptes);

    Task<IDictionary<byte, int>> ObtenirNbEtudiantsParSpePourSessionEtudeDeProgramme(Guid sessionEtudeId,
        Guid programmeId);

    Task<IDictionary<byte, double>> ObtenirMoyenneNbEtudiantsParSpePourProgramme(Guid programmeId);

    Task<IEnumerable<IDictionary<byte, int>>> ObtenirNbEtudiantsParSpeDesTroisDernieresSessions(Guid sessionEtudeId,
        Guid
            programmeId);

    Task<int[]> ObtenirNbServicesAdaptesDesDixDernieresSessionsParProgramme(Guid programmeId);
    Task<int[]> ObtenirNbResidentsTemporairesDesDixDernieresSessionsParProgramme(Guid programmeId);
    Task<int[]> ObtenirNbAssujetisR18DesDixDernieresSessionsParProgramme(Guid programmeId);
}