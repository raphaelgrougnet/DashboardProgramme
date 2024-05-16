namespace Web.Features.Members;

public class MemberDto
{
    public Guid Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Active { get; set; } = default!;
    public string Password { get; set; } = default!;
    public List<string> Roles { get; set; } = default!;
    public List<Guid> Programmes { get; set; } = default!;
}