using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Application.Helpers;

using Domain.Common;

using Microsoft.AspNetCore.WebUtilities;

namespace Application.Extensions;

public static partial class StringExtensions
{
    public static string Base64UrlDecode(this string value)
    {
        return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(value));
    }

    public static string Base64UrlEncode(this string value)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
    }

    public static decimal ExtractPriceFromText(string? text)
    {
        decimal price = 0;
        Match match = PriceRegex().Match(text ?? string.Empty);

        if (!match.Success)
        {
            return price;
        }

        string matchedValue = match.Value;

        return decimal.TryParse(matchedValue.Replace("$", ""), out price) ? price : 0;
    }

    public static string GetNameForCurrentTwoLetterLang(this TranslatableString? value)
    {
        return CultureHelper.GetCurrentTwoLetterLang() == "fr"
            ? value!.Fr
            : value!.En;
    }

    public static int IntTryParseOrZero(this string? value)
    {
        return int.TryParse(value, out int number) ? number : 0;
    }

    [GeneratedRegex(@"\$\d+(\.\d+)?|\d+(\.\d+)?\$")]
    private static partial Regex PriceRegex();

    public static string RemoveDiacritics(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        string normalizedValue = value.Normalize(NormalizationForm.FormD);
        StringBuilder db = new(normalizedValue.Length);

        int i = 0;
        while (i < normalizedValue.Length)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(normalizedValue[i]) != UnicodeCategory.NonSpacingMark)
            {
                db.Append(normalizedValue[i]);
            }

            i++;
        }

        return db.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    ///     Converts a string to base64 and returns an RFC 1342 compliant string. This ensures non ASCII characters will
    ///     be correctly interpreted by mail clients.
    /// </summary>
    public static string ToRfc1342Base64(this string value)
    {
        return $"=?UTF-8?B?{Convert.ToBase64String(Encoding.UTF8.GetBytes(value))}?=";
    }
}