namespace Application.Exceptions.IdentityError;

public class InvalidUserNameException(string message) : Exception(message);