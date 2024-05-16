namespace Application.Exceptions.IdentityError;

public class DuplicateEmailException(string message) : Exception(message);