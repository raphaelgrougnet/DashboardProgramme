namespace Application.Interfaces.Services.SessionAssistees;

public record SpeAggregate(
    IDictionary<byte, int> SessionActuelle,
    IDictionary<byte, double> MoyenneSessionsPrecedentes,
    IEnumerable<IDictionary<byte, int>> TroisDernieresSessions);