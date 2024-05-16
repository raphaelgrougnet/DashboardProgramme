namespace Web.Features.Programmes;

public class ProgrammeDto
{
    public Guid Id { get; set; } = default!;
    public string Nom { get; set; } = default!;
    public string Numero { get; set; } = default!;
}