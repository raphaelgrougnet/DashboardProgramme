namespace Web.Features.Correlations.CalculatePValueTourAdmission;

public class CalculatePValueTourAdmissionRequest
{
    public Guid IdCours { get; set; } = default!;
    public byte TourAdmission { get; set; } = default!;
}