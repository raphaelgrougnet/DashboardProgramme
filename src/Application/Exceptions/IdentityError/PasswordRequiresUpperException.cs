namespace Application.Exceptions.IdentityError;

public class PasswordRequiresUpperException(string message) : Exception(message);