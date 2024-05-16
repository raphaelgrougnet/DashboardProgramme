using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.Repositories.Users.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Users;

public class RoleRepository(RoleManager<Role> roleManager) : IRoleRepository
{
    public void Dispose()
    {
        roleManager.Dispose();
    }

    public async Task<Role> FindByName(string name)
    {
        Role? role = await roleManager.FindByNameAsync(name);
        if (role == null)
        {
            throw new RoleNotFoundException($"Could not find role with name {name} in database.");
        }

        return role;
    }
}