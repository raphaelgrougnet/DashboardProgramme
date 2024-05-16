using System.Diagnostics.CodeAnalysis;

using Application.Extensions;
using Application.Settings;

using Core.Flash;

using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Web.Extensions;
using Web.ViewModels.ResetPassword;

namespace Web.Controllers;

public class ResetPasswordController : BaseController
{
    private readonly IFlasher _flasher;
    private readonly IStringLocalizer<ResetPasswordController> _localizer;
    private readonly string _redirectUrl;
    private readonly IUserRepository _userRepository;

    public ResetPasswordController(
        IFlasher flasher,
        IUserRepository userRepository,
        ILogger<ResetPasswordController> logger,
        IOptions<ApplicationSettings> applicationSettings,
        IStringLocalizer<ResetPasswordController> localizer) : base(logger)
    {
        _flasher = flasher;
        _localizer = localizer;
        _userRepository = userRepository;
        _redirectUrl = applicationSettings.Value.RedirectUrl;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public IActionResult Index(string userId, string token)
    {
        return View(new ResetPasswordViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromQuery] string userId,
        [FromQuery] string token,
        [FromForm] ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool parsingSucceeded = Guid.TryParse(userId, out Guid id);
        if (!parsingSucceeded)
        {
            return BadRequest();
        }

        User? user = _userRepository.FindById(id);
        if (user == null)
        {
            return BadRequest();
        }

        IdentityResult identityResult =
            await _userRepository.UpdateUserPassword(user, model.Password, token.Base64UrlDecode());
        if (!identityResult.Succeeded)
        {
            _flasher.Flash(Types.Warning, identityResult.GetErrorMessageForIdentityResultException(_localizer), true);
            return View(model);
        }

        _flasher.Flash(Types.Success, _localizer["PasswordChangedFollowedByRedirect"], true);

        return View(new ResetPasswordViewModel { RedirectUrl = _redirectUrl });
    }
}