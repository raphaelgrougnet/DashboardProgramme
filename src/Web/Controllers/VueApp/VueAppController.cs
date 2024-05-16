using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Web.Cookies;

namespace Web.Controllers.VueAPp;

[Authorize]
[Route("/app")]
public class VueAppController : BaseController
{
    public VueAppController(ILogger<VueAppController> logger) : base(logger)
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
        string lang = HttpContext.GetCookieValue(CookieNames.LanguageCookieName);
        return RedirectToAction("Index", lang == "fr" ? "FrenchVueApp" : "EnglishVueApp");
    }
}