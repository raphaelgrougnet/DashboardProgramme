namespace Application.Exceptions.Users;

public class UserNotFoundException(string message) : Exception(message);