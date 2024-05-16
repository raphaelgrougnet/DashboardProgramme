namespace Application.Exceptions.IdentityError;

public class UserAlreadyHasPasswordException(string message) : Exception(message);