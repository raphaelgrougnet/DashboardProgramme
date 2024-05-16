using Application.Interfaces.Services;
using Application.Services.Users;
using Application.Services.Users.Exceptions;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Identity;

using Tests.Application.TestCollections;
using Tests.Common.Builders;
using Tests.Common.Fixtures;

namespace Tests.Application.Services.Users;

[Collection(ApplicationTestCollection.NAME)]
public class AuthenticatedUserServiceTests
{
    private const string ANY_PASSWORD = "Qwerty123!";
    private const string OTHER_PASSWORD = "Qwerty1234!";
    private const string ANY_EMAIL = "john.jane.doe@gmail.com";
    private const string OTHER_EMAIL = "john.jane.doe1@gmail.com";
    private const string ANY_PHONE_NUMBER = "418-390-4444";

    private readonly AuthenticatedUserService _authenticatedUserService;
    private readonly Mock<IHttpContextUserService> _httpContextUserService;

    private readonly TestFixture _testFixture;

    private readonly UserBuilder _userBuilder;

    private readonly Mock<IUserRepository> _userRepository;

    public AuthenticatedUserServiceTests(TestFixture testFixture)
    {
        _testFixture = testFixture;

        _userBuilder = new UserBuilder();

        _userRepository = new Mock<IUserRepository>();
        _httpContextUserService = new Mock<IHttpContextUserService>();

        _authenticatedUserService = new AuthenticatedUserService(_testFixture.UserManager, _userRepository.Object,
            _httpContextUserService.Object);
    }

    private User GivenAuthenticatedUserExists()
    {
        User user = _userBuilder.Build();
        _httpContextUserService.Setup(x => x.UserEmail).Returns(user.Email);
        _userRepository.Setup(x => x.FindByEmail(user.Email, false)).Returns(user);
        return user;
    }

    [Fact]
    public async Task GivenIncorrectCurrentPassword_WhenChangeUserPassword_ThenReturnUnsuccessfulIdentityResult()
    {
        // Arrange
        GivenAuthenticatedUserExists();

        // Act
        IdentityResult identityResult =
            await _authenticatedUserService.ChangeUserPassword(OTHER_PASSWORD, OTHER_PASSWORD);

        // Assert
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GivenNoUserIsAuthenticated_WhenChangeUserEmail_ThenThrowChangeAuthenticatedUserEmailException()
    {
        // Act & assert
        await Assert.ThrowsAsync<ChangeAuthenticatedUserEmailException>(
            async () => await _authenticatedUserService.ChangeUserEmail(ANY_EMAIL));
    }


    [Fact]
    public async Task
        GivenNoUserIsAuthenticated_WhenChangeUserPhoneNumber_ThenThrowChangeAuthenticatedUserEmailException()
    {
        // Act & assert
        await Assert.ThrowsAsync<ChangeAuthenticatedPhoneNumberException>(
            async () => await _authenticatedUserService.ChangeUserPhoneNumber(ANY_PHONE_NUMBER));
    }

    [Fact]
    public async Task
        GivenNoUserWithSpecifiedEmailExists_WhenChangeUserPassword_ThenThrowChangeAuthenticatedUserPasswordException()
    {
        // Arrange 
        _httpContextUserService.Setup(x => x.UserEmail).Returns(ANY_EMAIL);

        // Act & assert
        await Assert.ThrowsAsync<ChangeAuthenticatedUserPasswordException>(
            async () => await _authenticatedUserService.ChangeUserPassword(ANY_PASSWORD, OTHER_PASSWORD));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task
        GivenNullEmptyOrWhitespaceCurrentPassword_WhenChangeUserPassword_ThenThrowChangeAuthenticatedUserPasswordException(
            string currentUserPassword)
    {
        // Act & assert
        await Assert.ThrowsAsync<ChangeAuthenticatedUserPasswordException>(
            async () => await _authenticatedUserService.ChangeUserPassword(currentUserPassword, ANY_PASSWORD));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task
        GivenNullEmptyOrWhitespaceNewPassword_WhenChangeUserPassword_ThenThrowChangeAuthenticatedUserPasswordException(
            string newPassword)
    {
        // Act & assert
        await Assert.ThrowsAsync<ChangeAuthenticatedUserPasswordException>(
            async () => await _authenticatedUserService.ChangeUserPassword(ANY_PASSWORD, newPassword));
    }

    [Fact]
    public async Task GivenUserIsAuthenticated_WhenChangeUserEmail_ThenReturnSuccessfulIdentityResult()
    {
        // Arrange
        User user = await _testFixture.GivenUserInUserManager();
        _httpContextUserService.Setup(x => x.UserEmail).Returns(user.Email);
        _userRepository.Setup(x => x.FindByEmail(user.Email, false)).Returns(user);

        // Act
        IdentityResult identityResult = await _authenticatedUserService.ChangeUserEmail(OTHER_EMAIL);

        // Assert
        identityResult.Succeeded.ShouldBeTrue();
        identityResult.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task GivenUserIsAuthenticated_WhenChangeUserPhoneNumber_ThenReturnSuccessfulIdentityResult()
    {
        // Arrange
        User user = await _testFixture.GivenUserInUserManager();
        _httpContextUserService.Setup(x => x.UserEmail).Returns(user.Email);
        _userRepository.Setup(x => x.FindByEmail(user.Email, false)).Returns(user);

        // Act
        IdentityResult identityResult = await _authenticatedUserService.ChangeUserPhoneNumber(ANY_PHONE_NUMBER);

        // Assert
        identityResult.Succeeded.ShouldBeTrue();
        identityResult.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        GivenUserWithGivenEmailExistsAndCurrentPasswordIsCorrect_WhenChangeUserPassword_ThenReturnSuccessfulIdentityResult()
    {
        // Arrange
        User user = await _testFixture.GivenUserInUserManager();
        _httpContextUserService.Setup(x => x.UserEmail).Returns(user.Email);
        _userRepository.Setup(x => x.FindByEmail(user.Email, false)).Returns(user);

        // Act
        IdentityResult identityResult =
            await _authenticatedUserService.ChangeUserPassword(ANY_PASSWORD, OTHER_PASSWORD);

        // Assert
        identityResult.Succeeded.ShouldBeTrue();
        identityResult.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void WhenGetAuthenticatedUser_ThenReturnUserWithSameEmailAsAuthenticatedUser()
    {
        // Arrange
        User user = GivenAuthenticatedUserExists();

        // Act
        User authenticatedUser = _authenticatedUserService.GetAuthenticatedUser();

        // Assert
        authenticatedUser!.Email.ShouldBe(user.Email);
    }
}