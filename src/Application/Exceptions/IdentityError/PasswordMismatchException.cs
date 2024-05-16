namespace Application.Exceptions.IdentityError;

public class PasswordMismatchException(string message) : Exception(message);