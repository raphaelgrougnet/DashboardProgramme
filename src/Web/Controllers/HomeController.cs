using Microsoft.AspNetCore.Mvc;

using Web.Extensions;

namespace Web.Controllers;

[Route("")]
public class HomeController : BaseController
{
    public HomeController(ILogger<HomeController> logger) : base(logger)
    {
    }

    // GET
    public IActionResult Index()
    {
        return HttpContext.IsAuthenticated()
            ? RedirectToAction("Index", "VueApp")
            : RedirectToAction("Login", "Authentication");
    }
}