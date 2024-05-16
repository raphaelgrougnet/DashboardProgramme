using Application.Interfaces.Services.SessionAssistees;

using Domain.Repositories;

namespace Application.Services.SessionAssistees;

public class SpeQueryService(ISessionAssisteeRepository sessionAssisteeRepository) : ISpeQueryService
{
    public async Task<SpeAggregate> ObtenirSpeAggregate(Guid sessionEtudeId, Guid programmeId)
    {
        IDictionary<byte, int> sessionActuelle =
            await sessionAssisteeRepository.ObtenirNbEtudiantsParSpePourSessionEtudeDeProgramme(sessionEtudeId,
                programmeId);

        IDictionary<byte, double> moyenneSessionsPrecedentes =
            await sessionAssisteeRepository.ObtenirMoyenneNbEtudiantsParSpePourProgramme(programmeId);

        IEnumerable<IDictionary<byte, int>> troisDernieresSessions =
            await sessionAssisteeRepository.ObtenirNbEtudiantsParSpeDesTroisDernieresSessions(sessionEtudeId,
                programmeId);

        return new SpeAggregate(sessionActuelle, moyenneSessionsPrecedentes, troisDernieresSessions);
    }
}