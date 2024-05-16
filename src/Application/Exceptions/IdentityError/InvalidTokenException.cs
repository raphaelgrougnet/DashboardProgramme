namespace Application.Exceptions.IdentityError;

public class InvalidTokenException(string message) : Exception(message);