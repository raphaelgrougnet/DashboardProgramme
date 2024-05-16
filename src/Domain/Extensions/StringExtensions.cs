namespace Domain.Extensions;

public static class StringExtensions
{
    public static string? CapitalizeFirstLetterOfEachWord(this string? value)
    {
        string? lowercase = value?.ToLower();
        string? result = lowercase?
            .CapitalizeFirstLetterOfEachWordSeparatedBy(" ")
            .CapitalizeFirstLetterOfEachWordSeparatedBy("-")
            .CapitalizeFirstLetterOfEachWordSeparatedBy(".");

        if (result == null)
        {
            return result;
        }

        int openingParenthesis = result.IndexOf('(');
        int closingParenthesis = result.IndexOf(')');
        if (openingParenthesis == -1 || closingParenthesis == -1)
        {
            return result;
        }

        int startOfSubstring = openingParenthesis + 1;
        string wordInsideParenthesis = result.Substring(startOfSubstring, closingParenthesis - startOfSubstring);
        string wordInsideParenthesisFormatted = wordInsideParenthesis
            .CapitalizeFirstLetterOfEachWordSeparatedBy(" ")
            .CapitalizeFirstLetterOfEachWordSeparatedBy("-")
            .CapitalizeFirstLetterOfEachWordSeparatedBy(".");
        return result.Replace(wordInsideParenthesis, wordInsideParenthesisFormatted);
    }

    private static string CapitalizeFirstLetterOfEachWordSeparatedBy(this string value, string separator)
    {
        string[] splitBySeparator = value.Split(separator);
        IEnumerable<string> firstLetterOfWordUpperCase = splitBySeparator.Select(x =>
        {
            if (x.Length == 0)
            {
                return x;
            }

            return x.Length == 1 ? x.ToUpper() : x[..1].ToUpper() + x[1..];
        });
        return string.Join(separator, firstLetterOfWordUpperCase);
    }
}