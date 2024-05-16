namespace Web.Features.Admins.Members.CreateMember;

public class CreateMemberRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;
    public List<Guid> Programmes { get; set; } = default!;
}