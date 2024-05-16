// ReSharper disable LocalizableElement

namespace Domain.Common;

public class TranslatableString
{
    public const string DB_DEFAULT_VALUE = "{\"En\": \"\", \"Fr\": \"\"}";

    public TranslatableString() { }

    public TranslatableString(string fr, string en)
    {
        Fr = fr;
        En = en;
    }

    public string Fr { get; set; } = default!;
    public string En { get; set; } = default!;

    public bool Equals(TranslatableString other)
    {
        return Fr == other.Fr && En == other.En;
    }

    private string GetLookupLanguage(string? language)
    {
        string lookup = string.IsNullOrWhiteSpace(language)
            ? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName
            : language.ToLowerInvariant();

        // `iv` is a special case where a build machine does not have a default culture set
        return lookup == "iv" ? "en" : lookup;
    }

    public bool IsEmpty(bool partially = false)
    {
        return partially
            ? string.IsNullOrWhiteSpace(Fr) || string.IsNullOrWhiteSpace(En)
            : string.IsNullOrWhiteSpace(Fr) && string.IsNullOrWhiteSpace(En);
    }

    public override string ToString()
    {
        return Transl8();
    }

    public string Transl8(string? language = null)
    {
        string lookupLanguage = GetLookupLanguage(language);

        switch (lookupLanguage)
        {
            case "fr":
                return Fr;
            case "en":
                return En;
            default:
                if (string.IsNullOrWhiteSpace(language))
                {
                    throw new Exception($"Could not get translation. Language \"{lookupLanguage}\" " +
                                        "(current UI culture) is not managed.");
                }

                throw new ArgumentException(
                    $"Could not get translation. Language \"{lookupLanguage}\" is not managed.",
                    nameof(language));
        }
    }
}