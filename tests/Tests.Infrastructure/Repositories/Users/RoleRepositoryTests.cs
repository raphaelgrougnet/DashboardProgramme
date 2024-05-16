using Domain.Constants.User;
using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.Repositories.Users;
using Infrastructure.Repositories.Users.Exceptions;

using Tests.Common.Fixtures;
using Tests.Infrastructure.TestCollections;

namespace Tests.Infrastructure.Repositories.Users;

[Collection(InfrastructureTestCollection.NAME)]
public class RoleRepositoryTests(TestFixture testFixture)
{
    private readonly IRoleRepository _roleRepository = new RoleRepository(testFixture.RoleManager);

    [Fact]
    public async Task GivenNoRoleWithNameExists_WhenFindByName_ThenThrowRoleNotFoundException()
    {
        // Arrange
        const string INEXISTANT_ROLE_NAME = "inexistant role";

        // Act & assert
        RoleNotFoundException exception = await Assert.ThrowsAsync<RoleNotFoundException>(
            async () => await _roleRepository.FindByName(INEXISTANT_ROLE_NAME));
        exception.Message.ShouldBe($"Could not find role with name {INEXISTANT_ROLE_NAME} in database.");
    }

    [Fact]
    public async Task GivenRoleWithNameExists_WhenFindByName_ThenReturnMatchingRole()
    {
        // Act
        Role individualRole = await _roleRepository.FindByName(Roles.ADMINISTRATOR);

        // Assert
        individualRole.Name.ShouldBe(Roles.ADMINISTRATOR);
    }
}