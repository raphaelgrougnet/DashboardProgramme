namespace Application.Interfaces.Services.SessionAssistees;

public interface ISpeQueryService
{
    Task<SpeAggregate> ObtenirSpeAggregate(Guid sessionEtudeId, Guid programmeId);
}