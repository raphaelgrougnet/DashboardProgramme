namespace Web.Cookies;

public static class CookieHelper
{
    public static string GetCookieValue(this HttpContext httpContext, string cookieName)
    {
        if (string.IsNullOrWhiteSpace(cookieName))
        {
            return string.Empty;
        }

        return (httpContext.Request.Cookies.ContainsKey(cookieName)
            ? httpContext.Request.Cookies[cookieName]
            : string.Empty)!;
    }

    public static void SetCookieValue(
        this HttpContext httpContext,
        string cookieName,
        string cookieValue,
        bool secure,
        bool httpOnly)
    {
        if (string.IsNullOrWhiteSpace(cookieName))
        {
            return;
        }

        CookieOptions cookieOptions = new()
        {
            Domain = httpContext.Request.Host.Host,
            Path = "/",
            Secure = secure,
            HttpOnly = httpOnly,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        };

        httpContext.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
    }
}