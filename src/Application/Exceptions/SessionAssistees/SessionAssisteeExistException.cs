namespace Application.Exceptions.SessionAssistees;

public class SessionAssisteeExistException : Exception
{
    public SessionAssisteeExistException(string message) : base(message)
    {
    }
}