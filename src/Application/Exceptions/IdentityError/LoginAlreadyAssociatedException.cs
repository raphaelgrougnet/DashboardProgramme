namespace Application.Exceptions.IdentityError;

public class LoginAlreadyAssociatedException(string message) : Exception(message);