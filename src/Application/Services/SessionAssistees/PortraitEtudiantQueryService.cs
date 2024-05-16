using Application.Interfaces.Services.SessionAssistees;

using Domain.Repositories;

namespace Application.Services.SessionAssistees;

public class PortraitEtudiantQueryService(ISessionAssisteeRepository sessionAssisteeRepository)
    : IPortraitEtudiantQueryService
{
    public async Task<PortraitEtudiantAggregate> ObtenirPortraitEtudiantAggregate(Guid programmeId)
    {
        int[] SA =
            await sessionAssisteeRepository.ObtenirNbServicesAdaptesDesDixDernieresSessionsParProgramme(programmeId);

        int[] RT =
            await sessionAssisteeRepository.ObtenirNbResidentsTemporairesDesDixDernieresSessionsParProgramme(
                programmeId);

        int[] R18 =
            await sessionAssisteeRepository.ObtenirNbAssujetisR18DesDixDernieresSessionsParProgramme(programmeId);

        return new PortraitEtudiantAggregate(SA, RT, R18);
    }
}