using Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services.Users;

public interface IAuthenticatedUserService : IDisposable
{
    User? GetAuthenticatedUser();
    Task<IdentityResult> ChangeUserPassword(string currentPassword, string newPassword);
    Task<IdentityResult> ChangeUserEmail(string newEmail);
    Task<IdentityResult> ChangeUserPhoneNumber(string newPhoneNumber, int? newPhoneExtension = null);
}