namespace Application.Exceptions.IdentityError;

public class UserNotInRoleException(string message) : Exception(message);