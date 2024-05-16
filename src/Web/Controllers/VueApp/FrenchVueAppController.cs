using Application.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Cookies;
using Web.Extensions;

namespace Web.Controllers.VueAPp;

[Authorize]
[Route("/fr")]
public class FrenchVueAppController(ILogger<FrenchVueAppController> logger) : BaseController(logger)
{
    private void ChangeCultureToFrench()
    {
        CultureHelper.ChangeCurrentCultureTo("fr");
        HttpContext.SetAspNetLanguageCookie("fr");
        HttpContext.SetCookieValue(CookieNames.LanguageCookieName, "fr", false, false);
    }

    [HttpGet]
    public IActionResult Index()
    {
        ChangeCultureToFrench();
        ViewData["CurrentUrl"] = null;

        return View("VueAppPage");
    }

    [HttpGet]
    [Route("{**catchall}")]
    public IActionResult RedirectToSamePath(string catchall)
    {
        ChangeCultureToFrench();
        ViewData["CurrentUrl"] = Request.Path.Value;

        return View("VueAppPage");
    }
}