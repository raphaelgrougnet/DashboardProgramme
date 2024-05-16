namespace Application.Exceptions.IdentityError;

public class PasswordRequiresNonAlphanumericException(string message) : Exception(message);