using Application.Interfaces.Services.SessionEtudes;

using Domain.Entities.SessionEtudes;
using Domain.Repositories;

namespace Application.Services.SessionEtudes;

public class SessionEtudeCreationService(ISessionEtudeRepository sessionEtudeRepository) : ISessionEtudeCreationService
{
    public SessionEtude? FindByAnneeAndSaison(ushort annee, Saison saison)
    {
        return sessionEtudeRepository.FindByAnneeAndSaison(annee, saison);
    }
}