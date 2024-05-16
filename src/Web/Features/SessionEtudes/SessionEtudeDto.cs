namespace Web.Features.SessionEtudes;

public class SessionEtudeDto
{
    public Guid Id { get; set; } = default!;
    public ushort Annee { get; set; } = default!;
    public string Saison { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public ushort Ordre { get; set; } = default!;
}