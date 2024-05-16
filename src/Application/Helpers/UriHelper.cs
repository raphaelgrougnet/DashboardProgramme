﻿namespace Application.Helpers;

public static class UriHelper
{
    public static bool IsValidUri(this string? uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out Uri? uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}