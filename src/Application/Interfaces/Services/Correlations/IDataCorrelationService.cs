namespace Application.Interfaces.Services.Correlations;

public interface IDataCorrelationService
{
    Task<double> CalculatePValue(Guid idCours, string critere);

    Task<double> CalculatePValueGENMELS(Guid idCours);

    Task<double> CalculatePValueTourAdmission(Guid idCours, byte tourAdmission);

    Task<double> CalculatePValueInternational(Guid idCours, string etudiantInternational);
}