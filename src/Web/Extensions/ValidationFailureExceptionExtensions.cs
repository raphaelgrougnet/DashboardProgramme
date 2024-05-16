using Application.Common;

using FastEndpoints;

namespace Web.Extensions;

public static class ValidationFailureExceptionExtensions
{
    public static IEnumerable<Error> ErrorObjects(this ValidationFailureException exception)
    {
        if (exception.Failures == null)
        {
            return new List<Error>();
        }

        return exception.Failures
            .Select(x => new Error(x.ErrorCode, x.ErrorMessage))
            .ToList();
    }
}