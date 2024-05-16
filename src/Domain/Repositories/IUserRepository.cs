using Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IUserRepository : IDisposable
{
    bool UserWithEmailExists(string email);
    User? FindById(Guid id);
    User? FindByUserName(string userName);
    User? FindByEmail(string email, bool includeDeleted = false);
    Task<User> SyncUser(User user);
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task<IdentityResult> DeleteUser(User user);
    Task<string> GetResetPasswordTokenForUser(User user);
    Task<IdentityResult> CreateUserPassword(User user, string password);
    Task<IdentityResult> UpdateUserPassword(User user, string password, string resetPasswordToken);
}