namespace Infrastructure.Repositories.Users.Exceptions;

public class RoleNotFoundException(string message) : Exception(message);