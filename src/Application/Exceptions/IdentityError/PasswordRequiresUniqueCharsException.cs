namespace Application.Exceptions.IdentityError;

public class PasswordRequiresUniqueCharsException(string message) : Exception(message);