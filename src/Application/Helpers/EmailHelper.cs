using System.Text.RegularExpressions;

namespace Application.Helpers;

public static partial class EmailHelper
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    public static bool IsValidEmail(this string? email)
    {
        return EmailRegex().IsMatch(email?.Trim() ?? "");
    }
}