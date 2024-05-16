using System.Globalization;
using System.Security.Principal;

using Application.Helpers;

using Microsoft.AspNetCore.Localization;

using Web.Cookies;

namespace Web.Extensions;

public static class HttpContextExtensions
{
    public static bool IsAuthenticated(this HttpContext httpContext)
    {
        IIdentity? userIdentity = httpContext.User.Identity;

        return userIdentity is { IsAuthenticated: true };
    }

    public static void SetAspNetLanguageCookie(this HttpContext httpContext, string twoLetterLang)
    {
        CultureInfo culture = CultureHelper.ConvertTwoLetterIsoToCultureInfo(twoLetterLang);
        string cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
        httpContext.SetCookieValue(CookieRequestCultureProvider.DefaultCookieName, cookieValue, true, true);
    }
}