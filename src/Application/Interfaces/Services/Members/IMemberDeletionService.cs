namespace Application.Interfaces.Services.Members;

public interface IMemberDeletionService
{
    Task DeleteMemberWithId(Guid id);
}