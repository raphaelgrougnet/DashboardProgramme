namespace Application.Exceptions.IdentityError;

public class InvalidEmailException(string message) : Exception(message);