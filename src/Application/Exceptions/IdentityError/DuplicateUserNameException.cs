namespace Application.Exceptions.IdentityError;

public class DuplicateUserNameException(string message) : Exception(message);