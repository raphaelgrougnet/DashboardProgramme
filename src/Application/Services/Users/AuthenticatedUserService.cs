using Application.Interfaces.Services;
using Application.Interfaces.Services.Users;
using Application.Services.Users.Exceptions;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Identity;

namespace Application.Services.Users;

public class AuthenticatedUserService(
    UserManager<User> userManager,
    IUserRepository userRepository,
    IHttpContextUserService httpContextUserService)
    : IAuthenticatedUserService
{
    public async Task<IdentityResult> ChangeUserEmail(string newEmail)
    {
        User? user = GetAuthenticatedUser();
        if (user == null)
        {
            throw new ChangeAuthenticatedUserEmailException(
                $"Could not find user with email {httpContextUserService.UserEmail}");
        }

        user.Email = newEmail;
        user.UserName = newEmail;
        user.EmailConfirmed = true;
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangeUserPassword(string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ChangeAuthenticatedUserPasswordException("Current and new password cannot be null");
        }

        string? currentUserEmail = httpContextUserService.UserEmail;
        User? user = userRepository.FindByEmail(currentUserEmail!);
        if (user == null)
        {
            throw new ChangeAuthenticatedUserPasswordException($"Could not find user with email {currentUserEmail}");
        }

        return await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<IdentityResult> ChangeUserPhoneNumber(string newPhoneNumber, int? newPhoneExtension = null)
    {
        User? user = GetAuthenticatedUser();
        if (user == null)
        {
            throw new ChangeAuthenticatedPhoneNumberException(
                $"Could not find user with email {httpContextUserService.UserEmail}");
        }

        user.SetPhoneExtension(newPhoneExtension);
        string? changePhoneNumberToken = await userManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber);
        return await userManager.ChangePhoneNumberAsync(user, newPhoneNumber, changePhoneNumberToken);
    }

    public void Dispose()
    {
        userManager.Dispose();
    }

    public User? GetAuthenticatedUser()
    {
        return userRepository.FindByEmail(httpContextUserService.UserEmail!);
    }
}