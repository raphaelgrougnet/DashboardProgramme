namespace Application.Interfaces.Services.SessionAssistees;

public interface IPortraitEtudiantQueryService
{
    Task<PortraitEtudiantAggregate> ObtenirPortraitEtudiantAggregate(Guid programmeId);
}