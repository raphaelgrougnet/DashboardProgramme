using System.Globalization;

using Application.Helpers.Exceptions;

namespace Application.Helpers;

public static class CultureHelper
{
    public const string DefaultTwoLetterLang = "fr";
    private static readonly string[] AllowedLanguages = ["en", "fr"];

    public static void ChangeCurrentCultureTo(string twoLetterLang)
    {
        Thread.CurrentThread.CurrentCulture = ConvertTwoLetterIsoToCultureInfo(twoLetterLang);
        Thread.CurrentThread.CurrentUICulture = ConvertTwoLetterIsoToCultureInfo(twoLetterLang);
    }

    public static CultureInfo ConvertCultureNameToCultureInfo(string cultureName)
    {
        return new CultureInfo(cultureName);
    }

    public static CultureInfo ConvertTwoLetterIsoToCultureInfo(string twoLetterCulture)
    {
        string cultureName = ConvertTwoLetterIsoToCultureName(twoLetterCulture);
        return ConvertCultureNameToCultureInfo(cultureName);
    }

    private static string ConvertTwoLetterIsoToCultureName(string twoLetterCulture)
    {
        string formattedTwoLetterCulture = FormatTwoLetterCulture(twoLetterCulture);
        return formattedTwoLetterCulture switch
        {
            "en" => "en-CA",
            "fr" => "fr-CA",
            _ => "en-CA"
        };
    }

    public static string FormatTwoLetterCulture(string twoLetterCulture)
    {
        if (string.IsNullOrWhiteSpace(twoLetterCulture))
        {
            twoLetterCulture = DefaultTwoLetterLang;
        }

        if (!AllowedLanguages.Contains(twoLetterCulture.ToLowerInvariant()))
        {
            throw new UnsupportedCultureException($"Culture {twoLetterCulture} is not supported");
        }

        return twoLetterCulture.ToLowerInvariant();
    }

    public static CultureInfo GetCurrentCulture()
    {
        return Thread.CurrentThread.CurrentCulture;
    }

    public static string GetCurrentTwoLetterLang()
    {
        return GetCurrentCulture().TwoLetterISOLanguageName;
    }

    public static CultureInfo GetDefaultCulture()
    {
        return ConvertTwoLetterIsoToCultureInfo(DefaultTwoLetterLang);
    }
}