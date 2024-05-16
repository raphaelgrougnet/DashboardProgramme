using Application.Interfaces.Services.Notifications;
using Application.Services.Notifications.Dtos;
using Application.Settings;

using Core.Flash;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tests.Common.Builders;
using Tests.Common.Fixtures;
using Tests.Web.TestCollections;

using Web.Controllers;
using Web.ViewModels.ForgotPassword;

namespace Tests.Web.Controllers;

[Collection(WebTestCollection.NAME)]
public class ForgotPasswordControllerTests
{
    private const string ANY_BASE_URL = "https://localhost:7101";
    private const string INEXISTANT_USER_EMAIL = "john.doe12@gmail.com";

    private readonly Mock<IFlasher> _flasher;

    private readonly ForgotPasswordController _forgotPasswordController;
    private readonly Mock<INotificationService> _notificationService;

    private readonly TestFixture _testFixture;

    private readonly UserBuilder _userBuilder;
    private readonly Mock<IUserRepository> _userRepository;

    public ForgotPasswordControllerTests(TestFixture testFixture)
    {
        _testFixture = testFixture;

        _userBuilder = new UserBuilder();

        _flasher = new Mock<IFlasher>();
        _userRepository = new Mock<IUserRepository>();
        _notificationService = new Mock<INotificationService>();
        Mock<IStringLocalizer<ForgotPasswordController>> localizer = new();

        Mock<IOptions<ApplicationSettings>> applicationSettings = new();
        applicationSettings.Setup(x => x.Value).Returns(new ApplicationSettings { BaseUrl = ANY_BASE_URL });
        Mock<ILogger<ForgotPasswordController>> logger = new();

        _forgotPasswordController = new ForgotPasswordController(
            logger.Object,
            _userRepository.Object,
            _flasher.Object,
            _notificationService.Object,
            applicationSettings.Object,
            localizer.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = BuildControllerServiceProvider().Object }
            }
        };
    }

    private Mock<IServiceProvider> BuildControllerServiceProvider()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        Mock<IUrlHelperFactory> urlHelperFactory = new();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
            .Returns(urlHelperFactory.Object);

        Mock<IUrlHelper> urlHelper = new();
        urlHelperFactory
            .Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper.Object);

        Mock<ITempDataDictionaryFactory> tempDataDictionaryFactory = new();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(ITempDataDictionaryFactory)))
            .Returns(tempDataDictionaryFactory.Object);

        return serviceProviderMock;
    }

    [Fact]
    public async Task
        GivenNoUserWithGivenUserNameExists_WhenPostIndex_ThenDisplaySuccessToShowAsLittleInfoToUserAsPossibleToPreventAttacks()
    {
        // Arrange
        ForgotPasswordViewModel viewModel = new() { UserName = INEXISTANT_USER_EMAIL };

        // Act
        await _forgotPasswordController.Index(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Success, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenNoUserWithGivenUserNameExists_WhenPostIndex_ThenReturnView()
    {
        // Arrange
        ForgotPasswordViewModel viewModel = new() { UserName = INEXISTANT_USER_EMAIL };

        // Act
        IActionResult actionResult = await _forgotPasswordController.Index(viewModel);

        // Assert
        actionResult.ShouldBeOfType<ViewResult>();
    }

    [Fact]
    public async Task GivenNullUserName_WhenPostIndex_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        ForgotPasswordViewModel viewModel = new();
        _forgotPasswordController.ModelState.AddModelError(nameof(viewModel.UserName), "");

        // Act
        await _forgotPasswordController.Index(viewModel);

        // Assert
        _forgotPasswordController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenSendingNotificationWasSuccessful_WhenPostIndex_ThenDisplaySuccessMessage()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userRepository.Setup(x => x.FindByUserName(user.UserName)).Returns(user);
        _userRepository.Setup(x => x.GetResetPasswordTokenForUser(user)).ReturnsAsync(string.Empty);
        ForgotPasswordViewModel viewModel = new() { UserName = user.Email };
        _notificationService
            .Setup(x => x.SendForgotPasswordNotification(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(new SendNotificationResponseDto(true));

        // Act
        await _forgotPasswordController.Index(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Success, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenSendingNotificationWasSuccessful_WhenPostIndex_ThenDisplayWarning()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userRepository.Setup(x => x.FindByUserName(user.UserName)).Returns(user);
        _userRepository.Setup(x => x.GetResetPasswordTokenForUser(user)).ReturnsAsync(string.Empty);
        ForgotPasswordViewModel viewModel = new() { UserName = user.Email };
        _notificationService
            .Setup(x => x.SendForgotPasswordNotification(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(new SendNotificationResponseDto(false));

        // Act
        await _forgotPasswordController.Index(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenUserWithGivenUserNameExists_WhenPostIndex_ThenSendForgotPasswordNotification()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userRepository.Setup(x => x.FindByUserName(user.UserName)).Returns(user);
        _userRepository.Setup(x => x.GetResetPasswordTokenForUser(user)).ReturnsAsync(string.Empty);
        ForgotPasswordViewModel viewModel = new() { UserName = user.Email };
        _notificationService
            .Setup(x => x.SendForgotPasswordNotification(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(new SendNotificationResponseDto(true));

        // Act
        await _forgotPasswordController.Index(viewModel);

        // Assert
        _notificationService.Verify(x => x.SendForgotPasswordNotification(It.IsAny<User>(), It.IsAny<string>()));
    }

    [Fact]
    public void WhenGetIndex_ThenReturnViewResult()
    {
        // Act
        IActionResult actionResult = _forgotPasswordController.Index();

        // Assert
        actionResult.ShouldBeOfType<ViewResult>();
    }
}