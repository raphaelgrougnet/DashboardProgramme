namespace Application.Common;

public class Error
{
    public Error() { }

    public Error(string errorType, string errorMessage)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public string ErrorType { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
}