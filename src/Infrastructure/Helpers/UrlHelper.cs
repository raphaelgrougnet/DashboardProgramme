namespace Infrastructure.Helpers;

public static class UrlHelper
{
    public static string BuildUriWithQueryParameters(string baseUrl, List<KeyValuePair<string, string>> queryParameters)
    {
        if (queryParameters.Count == 0)
        {
            return baseUrl;
        }

        string baseQueryUrl = baseUrl + "?";
        foreach (KeyValuePair<string, string> queryParameter in queryParameters)
        {
            bool isLast = queryParameters.Last().Equals(queryParameter);
            baseQueryUrl += queryParameter.Key + "=" + queryParameter.Value + (isLast ? "" : "&");
        }

        return baseQueryUrl;
    }
}