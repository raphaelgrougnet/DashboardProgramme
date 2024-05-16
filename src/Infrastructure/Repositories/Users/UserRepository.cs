using Application.Exceptions.Users;

using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.Repositories.Users.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<User> CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || user.RoleNames.Count == 0)
        {
            throw new CreateUserException("Could not create user since role or email is null.");
        }

        if (userManager.Users.Any(x => x.Email == user.Email))
        {
            throw new UserWithEmailAlreadyExistsException($"User with email {user.Email} already exists.");
        }

        await userManager.CreateAsync(user);
        await userManager.AddToRolesAsync(user, user.RoleNames);

        return await userManager.FindByEmailAsync(user.Email) ??
               throw new CreateUserException("Could not find user after creation.");
    }

    public async Task<IdentityResult> CreateUserPassword(User user, string password)
    {
        return await userManager.AddPasswordAsync(user, password);
    }

    public async Task<IdentityResult> DeleteUser(User user)
    {
        if (await userManager.FindByIdAsync(user.Id.ToString()) == null)
        {
            throw new UserNotFoundException($"User with id {user.Id} does not exist.");
        }

        user.SoftDelete();
        return await userManager.UpdateAsync(user);
    }

    public void Dispose()
    {
        userManager.Dispose();
    }

    public User? FindByEmail(string email, bool includeDeleted = false)
    {
        User? user = userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefault(x => x.Email != null && x.Email.ToLower() == email.ToLower());
        if (includeDeleted)
        {
            return user;
        }

        return user != null && user.IsActive() ? user : null;
    }

    public User? FindById(Guid id)
    {
        return userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Where(x => !x.Deleted.HasValue)
            .FirstOrDefault(x => x.Id == id);
    }

    public User? FindByUserName(string userName)
    {
        User? user = userManager.Users.FirstOrDefault(x => x.UserName != null && x.UserName.ToLower() ==
            userName.ToLower
                ());
        return user != null && user.IsActive() ? user : null;
    }

    public async Task<string> GetResetPasswordTokenForUser(User user)
    {
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<User> SyncUser(User user)
    {
        if (!userManager.Users.Any(x => x.Email == user.Email))
        {
            return await CreateUser(user);
        }

        return await UpdateUser(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || user.RoleNames.Count == 0)
        {
            throw new UpdateUserException("Could not update user either because user role or email is null.");
        }

        if (!userManager.Users.Any(x => x.Id == user.Id))
        {
            throw new UserNotFoundException($"User with id {user.Id} does not exist.");
        }

        if (userManager.Users.Any(x => x.Email == user.Email && x.Id != user.Id))
        {
            throw new UserWithEmailAlreadyExistsException(
                "Could not change this user's email since another user has same email.");
        }

        List<string> newRoles = user.RoleNames;
        IList<string>? roles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, roles);
        await userManager.AddToRolesAsync(user, newRoles);
        await userManager.UpdateAsync(user);

        return await userManager.FindByEmailAsync(user.Email) ??
               throw new UpdateUserException("Could not find user after update.");
    }

    public async Task<IdentityResult> UpdateUserPassword(User user, string password, string resetPasswordToken)
    {
        return await userManager.ResetPasswordAsync(user, resetPasswordToken, password);
    }

    public bool UserWithEmailExists(string email)
    {
        User? user = userManager.Users.FirstOrDefault(x => x.Email != null && x.Email.ToLower() == email.ToLower());
        return user != null && user.IsActive();
    }
}