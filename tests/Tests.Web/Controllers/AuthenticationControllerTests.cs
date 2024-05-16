using System.Security.Claims;

using Application.Extensions;
using Application.Interfaces.Services.Notifications;

using Core.Flash;

using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Tests.Common.Builders;
using Tests.Common.Fixtures;
using Tests.Web.TestCollections;

using Web.Controllers;
using Web.ViewModels.Authentication;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Tests.Web.Controllers;

[Collection(WebTestCollection.NAME)]
public class AuthenticationControllerTests
{
    private const string ANY_PASSWORD = "Qwerty123!";
    private const string OTHER_PASSWORD = "QwertyQwerty123!";
    private const string ANOTHER_PASSWORD = "123!Qwerty123!";
    private const string ANY_USERNAME = "john.doe@gmail.com";
    private const string INEXISTANT_USER_EMAIL = "john.doe12@gmail.com";
    private const string ANY_RETURN_URL = "/connect/authorize?";
    private const string ANY_CODE = "454689";
    private const string ANY_ERROR_MESSAGE = "Invalid code.";
    private const string ANY_ERROR_MESSAGE_KEY = "InvalidCode";

    private readonly AuthenticationController _authenticationController;

    private readonly Mock<IFlasher> _flasher;
    private readonly Mock<IMemberRepository> _memberRepository;
    private readonly Mock<INotificationService> _notificationService;
    private readonly Mock<SignInManager<User>> _signInManager;

    private readonly TestFixture _testFixture;
    private readonly UserBuilder _userBuilder;
    private readonly Mock<UserManager<User>> _userManager;

    public AuthenticationControllerTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _userBuilder = new UserBuilder();
        _testFixture.ResetBuilders();

        _flasher = new Mock<IFlasher>();
        _notificationService = new Mock<INotificationService>();
        Mock<ILogger<AuthenticationController>> logger = new();
        Mock<IStringLocalizer<AuthenticationController>> localizer = new();
        localizer
            .Setup(x => x[It.IsAny<string>()])
            .Returns(new LocalizedString(ANY_ERROR_MESSAGE_KEY, ANY_ERROR_MESSAGE));
        Mock<IUserStore<User>> store = new();
        _userManager =
            new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _signInManager = new Mock<SignInManager<User>>(
            _userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null!, null!, null!, null!);
        _memberRepository = new Mock<IMemberRepository>();

        _authenticationController = new AuthenticationController(
            logger.Object,
            _flasher.Object,
            _signInManager.Object,
            _memberRepository.Object,
            _notificationService.Object,
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
        Mock<IAuthenticationService> authenticationServiceMock = new();
        authenticationServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        Mock<IServiceProvider> serviceProviderMock = new();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(IAuthenticationService)))
            .Returns(authenticationServiceMock.Object);

        Mock<IUrlHelperFactory> urlHelperFactory = new();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
            .Returns(urlHelperFactory.Object);

        Mock<ITempDataDictionaryFactory> tempDataDictionaryFactory = new();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(ITempDataDictionaryFactory)))
            .Returns(tempDataDictionaryFactory.Object);

        return serviceProviderMock;
    }

    [Fact]
    public async Task GivenCheckPasswordFailed_WhenPostLogin_ThenDisplayWarning()
    {
        // Arrange
        User user = GivenUserExistsAndHasMember();
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
            .ReturnsAsync(SignInResult.Failed);

        LoginViewModel viewModel = new()
        {
            UserName = user.UserName, Password = ANY_PASSWORD, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenCheckPasswordFailed_WhenPostLogin_ThenReturnViewWithSameTwoFactorViewModel()
    {
        // Arrange
        User user = GivenUserExistsAndHasMember();
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
            .ReturnsAsync(SignInResult.Failed);
        LoginViewModel expectedViewModel = new()
        {
            UserName = user.UserName, Password = ANY_PASSWORD, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        IActionResult actionResult = await _authenticationController.Login(expectedViewModel);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        LoginViewModel actualViewModel = viewResult.Model.ShouldBeOfType<LoginViewModel>();
        actualViewModel.UserName.ShouldBe(expectedViewModel.UserName);
        actualViewModel.Password.ShouldBe(expectedViewModel.Password);
        actualViewModel.ReturnUrl.ShouldBe(expectedViewModel.ReturnUrl);
    }

    [Fact]
    public async Task GivenNoUserWithEmailExists_WhenGetTwoFactorAuthentication_ThenRedirectToLogin()
    {
        // Act
        IActionResult actionResult =
            await _authenticationController.TwoFactorAuthentication(INEXISTANT_USER_EMAIL, null!);

        // Assert
        actionResult.ShouldBeOfType<RedirectToActionResult>().ActionName.ShouldBe("Login");
    }

    [Fact]
    public async Task GivenNoUserWithUserNameExists_WhenPostLogin_ThenDisplayWarning()
    {
        // Arrange
        LoginViewModel viewModel = new()
        {
            UserName = INEXISTANT_USER_EMAIL, Password = null!, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenNoUserWithUserNameExists_WhenPostTwoFactorAuthentication_ThenDisplayWarning()
    {
        // Arrange
        TwoFactorViewModel viewModel = new() { UserName = INEXISTANT_USER_EMAIL, TwoFactorCode = ANY_CODE };

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenNullPassword_WhenPostLogin_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        User user = await _testFixture.GivenUserInDatabase();
        LoginViewModel viewModel = new() { UserName = user.UserName, Password = null!, ReturnUrl = ANY_RETURN_URL };
        _authenticationController.ModelState.AddModelError(nameof(viewModel.Password), "");

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _authenticationController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenNullTwoFactorCode_WhenPostTwoFactorAuthentication_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        TwoFactorViewModel viewModel = new() { UserName = ANY_USERNAME, TwoFactorCode = null! };
        _authenticationController.ModelState.AddModelError(nameof(viewModel.TwoFactorCode), "");

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _authenticationController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenNullUserName_WhenPostLogin_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        LoginViewModel viewModel = new() { UserName = null!, Password = ANY_PASSWORD, ReturnUrl = ANY_RETURN_URL };
        _authenticationController.ModelState.AddModelError(nameof(viewModel.UserName), "");

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _authenticationController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenNullUserName_WhenPostTwoFactorAuthentication_ThenModelStateShouldNotBeValid()
    {
        // Arrange
        TwoFactorViewModel viewModel = new() { UserName = null!, TwoFactorCode = ANY_CODE };
        _authenticationController.ModelState.AddModelError(nameof(viewModel.UserName), "");

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _authenticationController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenPasswordRequiresTwoFactor_WhenPostLogin_ThenRedirectToTwoFactorAuthentication()
    {
        // Arrange
        User user = GivenUserExistsAndHasMember();
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), ANOTHER_PASSWORD, false))
            .ReturnsAsync(SignInResult.Success);
        _signInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), ANOTHER_PASSWORD, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);

        LoginViewModel viewModel = new()
        {
            UserName = user.UserName, Password = ANOTHER_PASSWORD, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        IActionResult actionResult = await _authenticationController.Login(viewModel);

        // Assert
        actionResult.ShouldBeOfType<RedirectToActionResult>().ActionName.ShouldBe("TwoFactorAuthentication");
    }

    [Fact]
    public async Task GivenPasswordSignInFails_WhenPostLogin_ThenReturnToView()
    {
        // Arrange
        User user = GivenUserExistsAndHasMember();
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false))
            .ReturnsAsync(SignInResult.Success);
        _signInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, true))
            .ReturnsAsync(SignInResult.Failed);

        LoginViewModel viewModel = new()
        {
            UserName = user.UserName, Password = ANY_PASSWORD, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        IActionResult actionResult = await _authenticationController.Login(viewModel);

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Index");
        redirectToActionResult.ControllerName.ShouldBe("VueApp");
    }

    [Fact]
    public async Task GivenPasswordSignInSucceeded_WhenPostLogin_ThenRedirectToVueAppPageView()
    {
        // Arrange
        User user = GivenUserExistsAndHasMember();
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), OTHER_PASSWORD, false))
            .ReturnsAsync(SignInResult.Success);
        _signInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), OTHER_PASSWORD, false, true))
            .ReturnsAsync(SignInResult.Success);

        LoginViewModel viewModel = new()
        {
            UserName = user.UserName, Password = OTHER_PASSWORD, ReturnUrl = ANY_RETURN_URL
        };

        // Act
        IActionResult actionResult = await _authenticationController.Login(viewModel);

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Index");
        redirectToActionResult.ControllerName.ShouldBe("VueApp");
    }

    [Fact]
    public async Task GivenTwoFactorSignInFails_WhenPostTwoFactorAuthentication_ThenAddModelError()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        TwoFactorViewModel viewModel = new()
        {
            UserName = user.Email, TwoFactorCode = ANY_CODE, ReturnUrl = ANY_RETURN_URL
        };
        _signInManager
            .Setup(x => x.TwoFactorSignInAsync("Email", viewModel.TwoFactorCode, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _authenticationController.ModelState.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenTwoFactorSignInFails_WhenPostTwoFactorAuthentication_ThenDisplayWarning()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        TwoFactorViewModel viewModel = new()
        {
            UserName = user.Email, TwoFactorCode = ANY_CODE, ReturnUrl = ANY_RETURN_URL
        };
        _signInManager
            .Setup(x => x.TwoFactorSignInAsync("Email", viewModel.TwoFactorCode, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task
        GivenTwoFactorSignInFails_WhenPostTwoFactorAuthentication_ThenReturnViewWithSameTwoFactorViewModel()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        TwoFactorViewModel expectedViewModel = new()
        {
            UserName = user.Email, TwoFactorCode = ANY_CODE, ReturnUrl = ANY_RETURN_URL
        };
        _signInManager
            .Setup(x => x.TwoFactorSignInAsync("Email", expectedViewModel.TwoFactorCode, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        IActionResult actionResult = await _authenticationController.TwoFactorAuthentication(expectedViewModel);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        TwoFactorViewModel actualViewModel = viewResult.Model.ShouldBeOfType<TwoFactorViewModel>();
        actualViewModel.UserName.ShouldBe(expectedViewModel.UserName);
        actualViewModel.TwoFactorCode.ShouldBe(expectedViewModel.TwoFactorCode);
        actualViewModel.ReturnUrl.ShouldBe(expectedViewModel.ReturnUrl);
    }

    [Fact]
    public async Task GivenTwoFactorSignInSucceedsAndReturnUrlIsSet_WhenPostTwoFactorAuthentication_ThenRedirectToHome()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        TwoFactorViewModel viewModel = new()
        {
            UserName = user.Email, TwoFactorCode = ANY_CODE, ReturnUrl = ANY_RETURN_URL
        };
        _signInManager
            .Setup(x => x.TwoFactorSignInAsync("Email", viewModel.TwoFactorCode, false, false))
            .ReturnsAsync(SignInResult.Success);

        // Act
        IActionResult actionResult = await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Index");
        redirectToActionResult.ControllerName.ShouldBe("VueApp");
    }

    private User GivenUserExistsAndHasMember()
    {
        User user = _userBuilder.WithId(Guid.NewGuid()).WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        Member member = _testFixture.MemberBuilder.Build();
        _memberRepository.Setup(x => x.FindByUserId(user.Id, true)).Returns(member);
        return user;
    }

    [Fact]
    public async Task GivenUserWithUserNameExistsButIsLinkedToNoMember_WhenPostLogin_ThenDisplayWarning()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        LoginViewModel viewModel = new() { UserName = user.Email, Password = null!, ReturnUrl = ANY_RETURN_URL };

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenUserWithUserNameWasDeleted_WhenPostLogin_ThenDisplayWarning()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        LoginViewModel viewModel = new() { UserName = user.Email, Password = null!, ReturnUrl = ANY_RETURN_URL };

        // Act
        await _authenticationController.Login(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public async Task GivenUserWithUserNameWasDeleted_WhenPostTwoFactorAuthentication_ThenDisplayWarning()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).AsDeleted().Build();
        _userManager.Setup(x => x.Users).Returns(user.IntoList().AsQueryable);
        TwoFactorViewModel viewModel = new() { UserName = user.Email, TwoFactorCode = ANY_CODE };

        // Act
        await _authenticationController.TwoFactorAuthentication(viewModel);

        // Assert
        _flasher.Verify(x => x.Flash(Types.Warning, It.IsAny<string>(), true));
    }

    [Fact]
    public void WhenGetLogin_ThenReturnLoginViewModelLoginViewModelReturnUrlPropertyIsSameAsGivenUrl()
    {
        // Act
        ViewResult? viewResult = _authenticationController.Login(ANY_RETURN_URL) as ViewResult;

        // Assert
        viewResult?.Model.ShouldBeOfType<LoginViewModel>().ReturnUrl.ShouldBe(ANY_RETURN_URL);
    }

    [Fact]
    public void WhenGetLogin_ThenReturnViewResult()
    {
        // Act
        IActionResult actionResult = _authenticationController.Login(ANY_RETURN_URL);

        // Assert
        actionResult.ShouldBeOfType<ViewResult>();
    }

    [Fact]
    public async Task WhenGetTwoFactorAuthentication_ThenReturnTwoFactorViewModelWithSpecifiedEmail()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);

        // Act
        IActionResult actionResult = await _authenticationController.TwoFactorAuthentication(user.Email, null!);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        TwoFactorViewModel viewModel = viewResult.Model.ShouldBeOfType<TwoFactorViewModel>();
        viewModel.UserName.ShouldBe(user.Email);
    }

    [Fact]
    public async Task WhenGetTwoFactorAuthentication_ThenReturnTwoFactorViewModelWithSpecifiedReturnUrl()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);

        // Act
        IActionResult actionResult =
            await _authenticationController.TwoFactorAuthentication(user.Email, ANY_RETURN_URL);

        // Assert
        ViewResult viewResult = actionResult.ShouldBeOfType<ViewResult>();
        TwoFactorViewModel viewModel = viewResult.Model.ShouldBeOfType<TwoFactorViewModel>();
        viewModel.ReturnUrl.ShouldBe(ANY_RETURN_URL);
    }

    [Fact]
    public async Task WhenGetTwoFactorAuthentication_ThenSendTwoFactorAuthenticationCode()
    {
        // Arrange
        User user = _userBuilder.WithEmail(_testFixture.GenerateEmail()).Build();
        _userManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);

        // Act
        await _authenticationController.TwoFactorAuthentication(user.Email, null!);

        // Assert
        _notificationService.Verify(x =>
            x.SendTwoFactorAuthenticationCodeNotification(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task WhenLogout_ThenRedirectToHome()
    {
        // Act
        IActionResult actionResult = await _authenticationController.Logout();

        // Assert
        RedirectToActionResult redirectToActionResult = actionResult.ShouldBeOfType<RedirectToActionResult>();
        redirectToActionResult.ActionName.ShouldBe("Login");
        redirectToActionResult.ControllerName.ShouldBe("Authentication");
    }
}