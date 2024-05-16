using Domain.Entities;

namespace Domain.Repositories;

public interface IMemberRepository
{
    int GetMemberCount();
    List<Member> GetAll();
    List<Member> GetAllWithUserEmail(string userEmail);
    Member FindById(Guid id);
    Member? FindByUserId(Guid userId, bool asNoTracking = true);
    Member? FindByUserEmail(string userEmail);
    Task<Member> CreateMember(string firstName, string lastName, string email, string password);
    Task UpdateMember(Member member);
    Task ClearRoles(Member member);
    Task AddRoleToMember(Member member, string role);
    
    Task DeleteMemberWithId(Guid id);

}