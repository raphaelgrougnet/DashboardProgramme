namespace Application.Exceptions.IdentityError;

public class PasswordTooShortException(string message) : Exception(message);