namespace Web.Features.Admins.Members.EditMember;

public class EditMemberRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Password { get; set; } = default!;
    public string Roles { get; set; } = default!;
    public List<Guid> Programmes { get; set; } = default!;
}