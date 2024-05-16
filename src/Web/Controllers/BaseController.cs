using Application.Helpers;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Web.Cookies;
using Web.Extensions;

namespace Web.Controllers;

public class BaseController(ILogger<BaseController> logger) : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string cookieValue = HttpContext.GetCookieValue(CookieRequestCultureProvider.DefaultCookieName);
        logger.Log(LogLevel.Information, $"Language value received from cookie : {cookieValue}");

        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            CultureHelper.ChangeCurrentCultureTo(CultureHelper.DefaultTwoLetterLang);
            HttpContext.SetAspNetLanguageCookie(CultureHelper.DefaultTwoLetterLang);
            HttpContext.SetCookieValue(CookieNames.LanguageCookieName, CultureHelper.DefaultTwoLetterLang, false,
                false);
        }
        else
        {
            ProviderCultureResult? cultures = CookieRequestCultureProvider.ParseCookieValue(cookieValue);
            if (cultures != null && cultures.Cultures.Any() && cultures.UICultures.Any())
            {
                string twoLetterLang = cultures.Cultures.First().Value?[..2] ?? CultureHelper.DefaultTwoLetterLang;
                CultureHelper.ChangeCurrentCultureTo(twoLetterLang);
                HttpContext.SetCookieValue(CookieNames.LanguageCookieName, twoLetterLang, false, false);
            }
        }

        base.OnActionExecuting(context);
    }
}