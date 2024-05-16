namespace Application.Interfaces.Services.Members;

public interface IMemberUpdateService
{
    Task UpdateMember(Guid id, string firstName, string lastName, string email, string? password, string? role,
        IEnumerable<Guid>? programmes);
}