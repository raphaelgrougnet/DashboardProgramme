namespace Application.Exceptions.IdentityError;

public class DuplicateRoleNameException(string message) : Exception(message);