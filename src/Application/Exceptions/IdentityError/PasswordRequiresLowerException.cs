namespace Application.Exceptions.IdentityError;

public class PasswordRequiresLowerException(string message) : Exception(message);