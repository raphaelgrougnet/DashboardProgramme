namespace Web.Features.Admins.Programmes.EditProgramme;

public class EditProgrammeRequest
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = default!;
    public string Numero { get; set; } = default!;
}