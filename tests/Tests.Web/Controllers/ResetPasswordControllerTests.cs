using Application.Extensions;
using Application.Settings;

using Core.Flash;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tests.Common.Builders;
using Tests.Common.Fixtures;
using Tests.Web.TestCollections;

using Web.Controllers;
using Web.ViewModels.ResetPassword;

namespace Tests.Web.Controllers;

[Collection(WebTestCollection.NAME)]
public class ResetPasswordControllerTests
{
    private const string ANY_RETURN_URL = "/";
    private const string VALID_PASSWORD = "Qwerty123!";
    private const string INVALID_PASSWORD = "qwerty";

    private readonly Mock<IFlasher> _flasher;

    private readonly ResetPasswordController _resetPasswordController;

    private readonly TestFixture _testFixture;

    private readonly UserBuilder _userBuilder;
    private readonly Mock<IUserRepository> _userRepository;

    public ResetPasswordControllerTests(TestFixture testFixture)
    {
        _testFixture = testFixture;

        _userBuilder = new UserBuilder();

        _flasher = new Mock<IFlasher>();
        _userRepository = new Mock<IUserRepository>();

        Mock<ILogger<ResetPasswordController>> logger = new();
        Mock<IStringLocalizer<ResetPasswordController>> localizer = new();
        Mock<IOptions<ApplicationSettings>> applicationSettings = new();
        applicationSettings.Setup(x => x.Value).Returns(new ApplicationSettings { RedirectUrl = ANY_RETURN_URL });

        _resetPasswordController = new ResetPasswordController(_flasher.Object, _userRepository.Object, logger.Object,
            applicationSettings.Object, localizer.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };
    }

    [Fact]
    public async Task GivenInvalidPassword_WhenPostIndex_ThenDisplayWarning()
    {
        // Arrange
        string encodedToken = string.Empty.Base64UrlEncode();
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userRepository.Setup(x => x.FindById(user.Id)).Returns(user);
        _userRepository
            .Setup(x => x.UpdateUserPassword(user, INVALID_PASSWORD, encodedToken.Base64UrlDecode()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));
        ResetPasswordViewModel viewModel = new() { Password = INVALID_PASSWORD };

        // Act
        await _resetPasswordController.Index(user.Id.ToString(), encodedToken, viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenNoUserWithIdExists_WhenPostIndex_ThenReturnBadRequest()
    {
        // Arrange
        string inexistantUserId = Guid.NewGuid().ToString();
        ResetPasswordViewModel viewModel = new() { Password = VALID_PASSWORD };

        // Act
        IActionResult result = await _resetPasswordController.Index(inexistantUserId, string.Empty, viewModel);

        // Assert
        result.ShouldBeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task GivenNullPassword_WhenPostIndex_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        ResetPasswordViewModel viewModel = new();
        _resetPasswordController.ModelState.AddModelError(nameof(viewModel.Password), "");

        // Act
        await _resetPasswordController.Index(string.Empty, string.Empty, viewModel);

        // Assert
        _resetPasswordController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void WhenGetIndex_ThenReturnViewResultWithResetPasswordViewMode()
    {
        // Act
        IActionResult actionResult = _resetPasswordController.Index(default!, default!);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        viewResult.Model.ShouldBeOfType<ResetPasswordViewModel>().ShouldNotBeNull();
    }

    [Fact]
    public async Task WhenPostIndex_ThenViewResultViewModelShouldHaveRedirectUrl()
    {
        // Arrange
        string encodedToken = string.Empty.Base64UrlEncode();
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userRepository.Setup(x => x.FindById(user.Id)).Returns(user);
        _userRepository
            .Setup(x => x.UpdateUserPassword(user, VALID_PASSWORD, encodedToken.Base64UrlDecode()))
            .ReturnsAsync(IdentityResult.Success);
        string userId = user.Id.ToString();
        ResetPasswordViewModel viewModel = new() { Password = VALID_PASSWORD };

        // Act
        IActionResult actionResult = await _resetPasswordController.Index(userId, encodedToken, viewModel);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        viewResult.Model.ShouldBeOfType<ResetPasswordViewModel>().RedirectUrl.ShouldBe(ANY_RETURN_URL);
    }
}