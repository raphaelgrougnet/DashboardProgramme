using Application.Exceptions.Users;
using Application.Extensions;

using Domain.Constants.User;
using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.Repositories.Users;
using Infrastructure.Repositories.Users.Exceptions;

using Microsoft.AspNetCore.Identity;

using Tests.Common.Builders;
using Tests.Common.Fixtures;
using Tests.Infrastructure.TestCollections;

namespace Tests.Infrastructure.Repositories.Users;

[Collection(InfrastructureTestCollection.NAME)]
public class UserRepositoryTests
{
    private readonly TestFixture _testFixture;
    private readonly UserBuilder _userBuilder;

    private readonly Mock<UserManager<User>> _userManager;

    private readonly IUserRepository _userRepository;

    public UserRepositoryTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _userBuilder = new UserBuilder();

        Mock<IUserStore<User>> store = new();
        _userManager =
            new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _userRepository = new UserRepository(_userManager.Object);
    }

    [Fact]
    public async Task
        GivenAnotherUserHasSameEmailAsRequestedEmail_WhenUpdateUser_ThenThrowUserWithEmailAlreadyExistsException()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userInDatabase = await _testFixture.GivenUserInDatabase();
        User user = _userBuilder.WithEmail(userInDatabase.Email).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(new List<User> { user, userInDatabase }.AsQueryable);

        // Act & assert
        await Assert.ThrowsAsync<UserWithEmailAlreadyExistsException>(
            async () => await _userRepository.UpdateUser(user));
    }

    [Fact]
    public void GivenIncludeDeletedIsFalseAndNoUserWithEmailExists_WhenFindByEmail_ThenReturnNull()
    {
        // Act
        User? actualUser = _userRepository.FindByEmail(_testFixture.GenerateEmail());

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public void GivenIncludeDeletedIsFalseAndUserWithEmailExists_WhenFindByEmail_ThenReturnMatchingUser()
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByEmail(expectedUser.UserName);

        // Assert
        actualUser.ShouldBe(expectedUser);
    }

    [Fact]
    public void GivenIncludeDeletedIsFalseAndUserWithEmailExistsButIsDeleted_WhenFindByEmail_ThenReturnNull()
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByEmail(expectedUser.UserName);

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public void GivenIncludeDeletedIsTrueAndNoUserWithEmailExists_WhenFindByEmail_ThenReturnNull()
    {
        // Act
        User? actualUser = _userRepository.FindByEmail(_testFixture.GenerateEmail(), true);

        // Assert
        actualUser.ShouldBeNull();
    }

    [Theory]
    [InlineData("garneau@spektrummedia.com")]
    [InlineData("GARNEAU@SPEKTRUMMEDIA.COM")]
    [InlineData("GARNEAU@spektrummedia.com")]
    public void GivenIncludeDeletedIsTrueAndUserWithEmailExists_WhenFindByEmail_ThenReturnMatchingUser(string email)
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(email).Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByEmail(email, true);

        // Assert
        actualUser.ShouldBe(expectedUser);
    }

    [Theory]
    [InlineData("garneau@spektrummedia.com")]
    [InlineData("GARNEAU@SPEKTRUMMEDIA.COM")]
    [InlineData("GARNEAU@spektrummedia.com")]
    public void GivenIncludeDeletedIsTrueAndUserWithEmailExistsButIsDeleted_WhenFindByEmail_ThenReturnMatchingUser(
        string email)
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(email).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByEmail(email, true);

        // Assert
        actualUser.ShouldBe(expectedUser);
    }

    [Fact]
    public void GivenNoUserWithEmailExists_WhenUserWithEmailExists_ThenReturnFalse()
    {
        // Act
        bool userWithEmailExists = _userRepository.UserWithEmailExists(_testFixture.GenerateEmail());

        // Assert
        userWithEmailExists.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenNoUserWithIdExists_WhenDeleteUser_ThenThrowUserNotFoundException()
    {
        // Arrange
        User? expectedUser = null;
        User user = _userBuilder.Build();
        _userManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(expectedUser);

        // Act & assert
        await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userRepository.DeleteUser(user));
    }

    [Fact]
    public void GivenNoUserWithIdExists_WhenFindById_ThenReturnNull()
    {
        // Act
        User? actualUser = _userRepository.FindById(Guid.NewGuid());

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public void GivenNoUserWithUserNameExists_WhenFindByUserName_ThenReturnNull()
    {
        // Act
        User? actualUser = _userRepository.FindByUserName(_testFixture.GenerateEmail());

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public async Task GivenUserWithEmailDoesNotExist_WhenSyncUser_ThenCreateUser()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToSync = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        _userManager.Setup(x => x.FindByEmailAsync(userToSync.Email!)).ReturnsAsync(userToSync);

        // Act
        await _userRepository.SyncUser(userToSync);

        // Assert
        _userManager.Verify(x => x.CreateAsync(It.IsAny<User>()));
    }

    private void GivenUserWithEmailExists(User user)
    {
        _userManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
    }

    [Fact]
    public async Task GivenUserWithEmailExists_WhenSyncUser_ThenUpdateUser()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
        _userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        await _userRepository.SyncUser(user);

        // Assert
        _userManager.Verify(x => x.UpdateAsync(It.IsAny<User>()));
    }

    [Theory]
    [InlineData("garneau@spektrummedia.com")]
    [InlineData("garneau@SPEKTRUMMEDIA.COM")]
    [InlineData("GARNEAU@spektrummedia.com")]
    public void GivenUserWithEmailExists_WhenUserWithEmailExists_ThenReturnTrue(string email)
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(email).Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        bool userWithEmailExists = _userRepository.UserWithEmailExists(email);

        // Assert
        userWithEmailExists.ShouldBeTrue();
    }

    [Fact]
    public void GivenUserWithEmailExistsButIsDeleted_WhenUserWithEmailExists_ThenReturnFalse()
    {
        // Arrange
        string email = _testFixture.GenerateEmail();
        User expectedUser = _userBuilder.WithEmail(email).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        bool userWithEmailExists = _userRepository.UserWithEmailExists(email);

        // Assert
        userWithEmailExists.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenUserWithIdExists_WhenDeleteUser_ThenDelegateUpdatingDeletedUserToUserManager()
    {
        // Arrange
        User user = _userBuilder.Build();
        _userManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);

        // Act
        await _userRepository.DeleteUser(user);

        // Assert
        _userManager.Verify(x => x.UpdateAsync(user));
    }

    [Fact]
    public async Task GivenUserWithIdExists_WhenDeleteUser_ThenSoftDeleteUser()
    {
        // Arrange
        User user = _userBuilder.Build();
        _userManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        User? updatedUser = null;
        _userManager
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(userParam => updatedUser = userParam);

        // Act
        await _userRepository.DeleteUser(user);

        // Assert
        updatedUser.ShouldNotBeNull();
        updatedUser.IsActive().ShouldBeFalse();
    }

    [Fact]
    public void GivenUserWithIdExists_WhenFindById_ThenReturnMatchingUser()
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindById(expectedUser.Id);

        // Assert
        actualUser.ShouldBe(expectedUser);
    }

    [Fact]
    public void GivenUserWithIdExistsButIsDeleted_WhenFindById_ThenReturnNull()
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindById(expectedUser.Id);

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public async Task GivenUserWithIdThatDoesNotExistInDatabase_WhenUpdateUser_ThenThrowUserNotFoundException()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();

        // Act & assert
        await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userRepository.UpdateUser(user));
    }

    [Fact]
    public async Task GivenUserWithNoRole_WhenUpdateUser_ThenThrowUpdateUserException()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();

        // Act & assert
        await Assert.ThrowsAsync<UpdateUserException>(async () => await _userRepository.UpdateUser(user));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GivenUserWithNullEmptyOrWhitespaceEmail_WhenCreateUser_ThenThrowCreateUserException(string? email)
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(email!).WithRole(role).Build();

        // Act & assert
        await Assert.ThrowsAsync<CreateUserException>(async () => await _userRepository.CreateUser(user));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GivenUserWithNullEmptyOrWhitespaceEmail_WhenUpdateUser_ThenThrowCreateUserException(string? email)
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(email!).WithRole(role).Build();

        // Act & assert
        await Assert.ThrowsAsync<UpdateUserException>(async () => await _userRepository.UpdateUser(user));
    }

    [Fact]
    public async Task GivenUserWithNullRole_WhenCreateUser_ThenThrowCreateUserException()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        user.UserRoles.Clear();

        // Act & assert
        await Assert.ThrowsAsync<CreateUserException>(async () => await _userRepository.CreateUser(user));
    }

    [Fact]
    public async Task GivenUserWithSameEmailExists_WhenCreateUser_ThenThrowUserWithEmailAlreadyExistsException()
    {
        // Arrange
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(email).WithRole(role).Build();
        User otherUser = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(new List<User> { user, otherUser }.AsQueryable);

        // Act & assert
        await Assert.ThrowsAsync<UserWithEmailAlreadyExistsException>(
            async () => await _userRepository.CreateUser(user));
    }

    [Theory]
    [InlineData("garneau@spektrummedia.com")]
    [InlineData("garneau@SPEKTRUMMEDIA.COM")]
    [InlineData("GARNEAU@spektrummedia.com")]
    public void GivenUserWithUserNameExists_WhenFindByUserName_ThenReturnMatchingUser(string username)
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(username).Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByUserName(username);

        // Assert
        actualUser.ShouldBe(expectedUser);
    }

    [Fact]
    public void GivenUserWithUserNameExistsButIsDeleted_WhenFindByUserName_ThenReturnNull()
    {
        // Arrange
        User expectedUser = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(expectedUser.IntoList().AsQueryable);

        // Act
        User? actualUser = _userRepository.FindByUserName(expectedUser.UserName);

        // Assert
        actualUser.ShouldBeNull();
    }

    [Fact]
    public async Task WhenCreateUser_ThenAddUserToItsRole()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToCreate = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        _userManager.Setup(x => x.FindByEmailAsync(userToCreate.Email!)).ReturnsAsync(userToCreate);

        // Act
        await _userRepository.CreateUser(userToCreate);

        // Assert
        _userManager.Verify(x => x.AddToRolesAsync(It.IsAny<User>(), Roles.ADMINISTRATOR.IntoList()));
    }

    [Fact]
    public async Task WhenCreateUser_ThenCreateUser()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToCreate = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        _userManager.Setup(x => x.FindByEmailAsync(userToCreate.Email!)).ReturnsAsync(userToCreate);

        // Act
        await _userRepository.CreateUser(userToCreate);

        // Assert
        _userManager.Verify(x => x.CreateAsync(It.IsAny<User>()));
    }

    [Fact]
    public async Task WhenCreateUser_ThenCreateWithGivenEmail()
    {
        // Arrange
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToCreate = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        GivenUserWithEmailExists(userToCreate);

        // Act
        User createdUser = await _userRepository.CreateUser(userToCreate);

        // Assert
        createdUser.Email.ShouldBe(email);
    }

    [Fact]
    public async Task WhenCreateUser_ThenCreateWithGivenRole()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToCreate = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        GivenUserWithEmailExists(userToCreate);

        // Act
        User createdUser = await _userRepository.CreateUser(userToCreate);

        // Assert
        createdUser.RoleNames.ShouldContain(Roles.ADMINISTRATOR);
    }

    [Fact]
    public async Task WhenCreateUser_ThenReturnCreatedUser()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToCreate = _userBuilder.WithEmail(_testFixture.GenerateEmail()).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        GivenUserWithEmailExists(userToCreate);

        // Act
        User createdUser = await _userRepository.CreateUser(userToCreate);

        // Assert
        createdUser.ShouldNotBeNull();
    }

    [Fact]
    public async Task WhenCreateUserPassword_ThenDelegateAddingPasswordToUserToUserManager()
    {
        // Arrange
        const string ANY_PASSSWORD = "my password";
        User user = _userBuilder.Build();

        // Act
        await _userRepository.CreateUserPassword(user, ANY_PASSSWORD);

        // Assert
        _userManager.Verify(x => x.AddPasswordAsync(user, ANY_PASSSWORD));
    }

    [Fact]
    public async Task WhenGetResetPasswordTokenForUser_ThenDelegateResetPasswordTokenGenerationToUserManager()
    {
        // Arrange
        User user = _userBuilder.Build();

        // Act
        await _userRepository.GetResetPasswordTokenForUser(user);

        // Assert
        _userManager.Verify(x => x.GeneratePasswordResetTokenAsync(user));
    }

    [Fact]
    public async Task WhenSyncUser_ThenReturnCreatedOrUpdatedUser()
    {
        // Arrange
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User userToSync = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable));
        GivenUserWithEmailExists(userToSync);

        // Act
        User createdUser = await _userRepository.SyncUser(userToSync);

        // Assert
        createdUser.ShouldNotBeNull();
        createdUser.Email.ShouldBe(email);
    }

    [Fact]
    public async Task WhenUpdateUser_ThenAddUserToItsRole()
    {
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
        _userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        await _userRepository.UpdateUser(user);

        // Act & assert
        _userManager.Verify(x => x.AddToRolesAsync(It.IsAny<User>(), Roles.ADMINISTRATOR.IntoList()));
    }

    [Fact]
    public async Task WhenUpdateUser_ThenRemoveUserFromExistingRoles()
    {
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        List<string> roles = Roles.ADMINISTRATOR.IntoList();
        User user = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
        _userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        await _userRepository.UpdateUser(user);

        // Act
        _userManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<User>(), roles));
    }

    [Fact]
    public async Task WhenUpdateUser_ThenUpdateUser()
    {
        string email = _testFixture.GenerateEmail();
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithEmail(email).WithRole(role).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());
        _userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        await _userRepository.UpdateUser(user);

        // Act & assert
        _userManager.Verify(x => x.UpdateAsync(It.IsAny<User>()));
    }

    [Fact]
    public async Task WhenUpdateUserPassword_ThenDelegateResettingUserPasswordToUserManager()
    {
        // Arrange
        const string ANY_TOKEN = "my token";
        const string ANY_PASSSWORD = "my new password";
        User user = _userBuilder.Build();

        // Act
        await _userRepository.UpdateUserPassword(user, ANY_PASSSWORD, ANY_TOKEN);

        // Assert
        _userManager.Verify(x => x.ResetPasswordAsync(user, ANY_TOKEN, ANY_PASSSWORD));
    }
}