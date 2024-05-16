namespace Application.Exceptions.IdentityError;

public class UserAlreadyInRoleException(string message) : Exception(message);