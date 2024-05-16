namespace Application.Services.Users.Exceptions;

public class ChangeAuthenticatedUserPasswordException(string message) : Exception(message);