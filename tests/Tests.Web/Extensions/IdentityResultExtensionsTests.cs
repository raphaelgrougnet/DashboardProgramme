using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

using Web.Constants;
using Web.Extensions;

namespace Tests.Web.Extensions;

public class IdentityResultExtensionsTests
{
    private readonly Mock<IStringLocalizer> _localizer = new();

    [Fact]
    public void
        GivenHasErrorOtherThanUserAlreadyHasPassword_WhenGetErrorMessageForIdentityResultException_ThenReturnMessageThatIsNotUserAlreadyHasPassword()
    {
        // Arrange
        const string ERROR_MESSAGE_KEY = "UserAlreadyHasPassword";
        const string ERROR_MESSAGE = "User already has a password.";
        _localizer.Setup(x => x[ERROR_MESSAGE_KEY]).Returns(new LocalizedString(ERROR_MESSAGE_KEY, ERROR_MESSAGE));
        IdentityError error = new() { Code = "OTHER_CODE" };
        IdentityResult? identityResult = IdentityResult.Failed(error);

        // Act
        string message = identityResult.GetErrorMessageForIdentityResultException(_localizer.Object);

        message.ShouldNotBe(ERROR_MESSAGE);
    }

    [Fact]
    public void
        GivenIdentityResultHasExceptionOfTypeUserAlreadyHasPassword_WhenGetErrorMessageForIdentityResultException_ThenReturnUserAlreadyHasPasswordMessage()
    {
        // Arrange
        const string ERROR_MESSAGE_KEY = "UserAlreadyHasPassword";
        const string ERROR_MESSAGE = "User already has a password.";
        _localizer.Setup(x => x[ERROR_MESSAGE_KEY]).Returns(new LocalizedString(ERROR_MESSAGE_KEY, ERROR_MESSAGE));
        IdentityError error = new() { Code = IdentityResultExceptions.USER_ALREADY_HAS_PASSWORD };
        IdentityResult? identityResult = IdentityResult.Failed(error);

        // Act
        string message = identityResult.GetErrorMessageForIdentityResultException(_localizer.Object);

        message.ShouldBe(ERROR_MESSAGE);
    }

    [Fact]
    public void
        GivenIdentityResultWithoutErrorCodes_WhenGetErrorMessageForIdentityResultException_ThenReturnCouldNotChangePasswordMessage()
    {
        // Arrange
        const string ERROR_MESSAGE_KEY = "CouldNotChangePassword";
        const string ERROR_MESSAGE = "Could not change password";
        _localizer.Setup(x => x[ERROR_MESSAGE_KEY]).Returns(new LocalizedString(ERROR_MESSAGE_KEY, ERROR_MESSAGE));
        IdentityResult identityResult = new();

        // Act
        string message = identityResult.GetErrorMessageForIdentityResultException(_localizer.Object);

        message.ShouldBe(ERROR_MESSAGE);
    }
}