using System.Security.Claims;

using Application.Interfaces.Services.Notifications;

using Core.Flash;

using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Web.ViewModels.Authentication;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Web.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IFlasher _flasher;
    private readonly IStringLocalizer<AuthenticationController> _localizer;
    private readonly IMemberRepository _memberRepository;
    private readonly INotificationService _notificationService;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IFlasher flasher,
        SignInManager<User> signInManager,
        IMemberRepository memberRepository,
        INotificationService notificationService,
        IStringLocalizer<AuthenticationController> localizer) : base(logger)
    {
        _flasher = flasher;
        _signInManager = signInManager;
        _memberRepository = memberRepository;
        _notificationService = notificationService;
        _localizer = localizer;
    }

    private async Task<IActionResult> AddClaimsAndSignInToHttpContext(User user)
    {
        if (user is { UserName: null } or { Email: null })
        {
            throw new InvalidOperationException("User must have a valid username and email.");
        }

        List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Email, user.Email)];
        claims.AddRange(user.UserRoles.Select(test => new Claim(ClaimTypes.Role, test.Role.Name!)));

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "VueApp");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.UserName = model.UserName.Trim();

        User? user = _signInManager.UserManager.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefault(u => u.NormalizedUserName == model.UserName.ToUpperInvariant());
        Member? member = user == null ? null : _memberRepository.FindByUserId(user.Id);

        if (user == null || !user.IsActive() || member == null)
        {
            _flasher.Flash(Types.Warning, _localizer["InvalidUsernameOrPassword"], true);
            return View(model);
        }

        SignInResult? checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!checkPasswordResult.Succeeded)
        {
            _flasher.Flash(Types.Warning, _localizer["InvalidUsernameOrPassword"], true);
            return View(model);
        }

        SignInResult? signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
        if (signInResult is { Succeeded: true })
        {
            return await AddClaimsAndSignInToHttpContext(user);
        }

        if (!signInResult.RequiresTwoFactor)
        {
            return RedirectToAction("Index", "VueApp");
        }

        var requestBody = new { user.Email, model.ReturnUrl };
        return RedirectToAction("TwoFactorAuthentication", requestBody);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Login", "Authentication");
    }


    [AllowAnonymous]
    public async Task<IActionResult> TwoFactorAuthentication(string email, string returnUrl)
    {
        User? user = await _signInManager.UserManager.FindByEmailAsync(email);
        switch (user)
        {
            case null:
                return RedirectToAction("Login");
            case { UserName: null } or { Email: null }:
                throw new InvalidOperationException("User must have a valid username and email.");
        }

        string? token = await _signInManager.UserManager.GenerateTwoFactorTokenAsync(user, "Email");
        await _notificationService.SendTwoFactorAuthenticationCodeNotification(user.Email, token);

        return View(new TwoFactorViewModel { UserName = user.UserName, ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> TwoFactorAuthentication(TwoFactorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user = _signInManager.UserManager.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(x => !x.Deleted.HasValue)
            .SingleOrDefault(u => u.NormalizedUserName == model.UserName.ToUpperInvariant());

        if (user == null)
        {
            _flasher.Flash(Types.Warning, _localizer["InvalidUsernameOrPassword"], true);
            return View(model.TwoFactorCode);
        }

        SignInResult? result = await _signInManager.TwoFactorSignInAsync("Email", model.TwoFactorCode, false, false);
        if (result.Succeeded)
        {
            return await AddClaimsAndSignInToHttpContext(user);
        }

        ModelState.AddModelError(model.TwoFactorCode, _localizer["InvalidCode"]);
        _flasher.Flash(Types.Warning, _localizer["InvalidCode"], true);
        return View(model);
    }
}