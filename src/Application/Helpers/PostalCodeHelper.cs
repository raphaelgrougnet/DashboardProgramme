using System.Text.RegularExpressions;

namespace Application.Helpers;

public static partial class PostalCodeHelper
{
    public static bool IsValidPostalCode(this string? postalCode)
    {
        return PostalCodeRegex().IsMatch(postalCode ?? "");
    }

    [GeneratedRegex(@"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$")]
    private static partial Regex PostalCodeRegex();
}