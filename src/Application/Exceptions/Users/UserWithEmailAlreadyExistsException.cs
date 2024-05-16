namespace Application.Exceptions.Users;

public class UserWithEmailAlreadyExistsException(string message) : Exception(message);