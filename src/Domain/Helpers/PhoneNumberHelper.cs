namespace Domain.Helpers;

public static class PhoneNumberHelper
{
    public const string EXTENSION_SEPARATOR = "poste";

    public static string? AddExtensionToPhoneNumber(string? phoneNumber, int? extension)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || extension == null)
        {
            return phoneNumber;
        }

        return $"{phoneNumber} {EXTENSION_SEPARATOR} {extension}";
    }

    public static int? FindExtensionInPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return null;
        }

        string[] phoneNumberParts = phoneNumber.Split(EXTENSION_SEPARATOR);
        if (phoneNumberParts.Length < 2 || phoneNumberParts.Any(x => string.IsNullOrWhiteSpace(x.Trim())))
        {
            return null;
        }

        string stringExtension = phoneNumberParts[1].Trim();
        return int.TryParse(stringExtension, out int extension) ? extension : null;
    }

    public static string? RemoveExtensionFromPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return null;
        }

        string[] phoneNumberParts = phoneNumber.Split(EXTENSION_SEPARATOR);
        return phoneNumberParts.Length == 0 ? phoneNumber : phoneNumberParts[0].Trim();
    }
}