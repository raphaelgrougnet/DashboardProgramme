namespace Web.Features.Correlations.CalculatePValue;

public class CalculatePValueRequest
{
    public Guid IdCours { get; set; } = default!;
    public string Critere { get; set; } = default!;
}