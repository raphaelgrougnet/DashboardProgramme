using Application.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Cookies;
using Web.Extensions;

namespace Web.Controllers.VueApp;

[Authorize]
[Route("/en")]
public class EnglishVueAppController : BaseController
{
    public EnglishVueAppController(ILogger<EnglishVueAppController> logger) : base(logger)
    {
    }

    private void ChangeCultureToEnglish()
    {
        CultureHelper.ChangeCurrentCultureTo("en");
        HttpContext.SetAspNetLanguageCookie("en");
        HttpContext.SetCookieValue(CookieNames.LanguageCookieName, "en", false, false);
    }

    [HttpGet]
    public IActionResult Index()
    {
        ChangeCultureToEnglish();
        ViewData["CurrentUrl"] = null;

        return View("VueAppPage");
    }

    [HttpGet]
    [Route("{**catchall}")]
    public IActionResult RedirectToSamePath(string catchall)
    {
        ChangeCultureToEnglish();
        ViewData["CurrentUrl"] = Request.Path.Value;

        return View("VueAppPage");
    }
}