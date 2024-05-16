namespace Application.Exceptions.IdentityError;

public class PasswordRequiresDigitException(string message) : Exception(message);