namespace Web.Features.Correlations.CalculatePValue;

public class CalculatePValueInternationalRequest
{
    public Guid IdCours { get; set; } = default!;
    public string EtudiantInternational { get; set; } = default!;
}