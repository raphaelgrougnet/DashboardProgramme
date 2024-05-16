using Application.Extensions;
using Application.Interfaces.Services.Notifications;
using Application.Services.Notifications.Dtos;
using Application.Settings;

using Core.Flash;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Web.ViewModels.ForgotPassword;

namespace Web.Controllers;

public class ForgotPasswordController : BaseController
{
    private readonly string _baseUrl;

    private readonly IFlasher _flasher;
    private readonly IStringLocalizer<ForgotPasswordController> _localizer;
    private readonly ILogger<ForgotPasswordController> _logger;
    private readonly INotificationService _notificationService;
    private readonly string _redirectUrl;
    private readonly IUserRepository _userRepository;

    public ForgotPasswordController(
        ILogger<ForgotPasswordController> logger,
        IUserRepository userRepository,
        IFlasher flasher,
        INotificationService notificationService,
        IOptions<ApplicationSettings> applicationSettings,
        IStringLocalizer<ForgotPasswordController> localizer) : base(logger)
    {
        _logger = logger;
        _flasher = flasher;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _baseUrl = applicationSettings.Value.BaseUrl;
        _redirectUrl = applicationSettings.Value.RedirectUrl;
        _localizer = localizer;
    }

    private async Task<string> BuildResetPasswordLink(User user)
    {
        string token = await _userRepository.GetResetPasswordTokenForUser(user);
        var actionObject = new { userId = user.Id, token = token.Base64UrlEncode(), redirectUrl = _redirectUrl };
        return _baseUrl + Url.Action("Index", "ResetPassword", actionObject)!;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ForgotPasswordViewModel forgotPasswordViewModel)
    {
        if (!ModelState.IsValid)
        {
            _flasher.Flash(Types.Warning, $"{_localizer["YouMustEnterYourEmail"]}", true);
            return View();
        }

        User? user = _userRepository.FindByUserName(forgotPasswordViewModel.UserName);
        if (user == null)
        {
            _flasher.Flash(Types.Success, $"{_localizer["NotificationSent"]} " +
                                          $"{forgotPasswordViewModel.UserName}.", true);
            return View();
        }

        string link = await BuildResetPasswordLink(user);
        SendNotificationResponseDto response = await _notificationService.SendForgotPasswordNotification(user, link);

        ShowSuccessOrError(response, user);

        return View();
    }

    private void ShowSuccessOrError(SendNotificationResponseDto response, User user)
    {
        if (response.Successful)
        {
            _flasher.Flash(Types.Success, $"{_localizer["NotificationSent"]}", true);
            _logger.LogInformation($"Email address confirmation email was successfully sent to {user.UserName}");
        }
        else
        {
            _flasher.Flash(Types.Warning, $"{_localizer["ErrorOccured"]} {user.UserName}.", true);
            _logger.LogError($"Could not send email address confirmation email to {user.UserName}\n" +
                             $"Errors: {string.Join("\n", response.ErrorMessages)}");
        }
    }
}