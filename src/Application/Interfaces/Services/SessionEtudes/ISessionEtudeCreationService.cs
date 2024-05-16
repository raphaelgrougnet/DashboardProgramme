using Domain.Entities.SessionEtudes;

namespace Application.Interfaces.Services.SessionEtudes;

public interface ISessionEtudeCreationService
{
    SessionEtude? FindByAnneeAndSaison(ushort annee, Saison saison);
}