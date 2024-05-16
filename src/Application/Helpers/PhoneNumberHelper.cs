using System.Text.RegularExpressions;

namespace Application.Helpers;

public static partial class PhoneNumberHelper
{
    public static bool IsValidPhoneNumber(this string? number)
    {
        return PhoneNumberRegex().IsMatch(number ?? "");
    }

    [GeneratedRegex("^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
    private static partial Regex PhoneNumberRegex();
}