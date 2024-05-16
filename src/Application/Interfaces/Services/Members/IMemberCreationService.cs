namespace Application.Interfaces.Services.Members;

public interface IMemberCreationService
{
    Task CreateMember(string firstName, string lastName, string email, string password, string? role,
        IEnumerable<Guid>? programmes);
}