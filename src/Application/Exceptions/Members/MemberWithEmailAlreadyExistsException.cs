namespace Application.Exceptions.Members;

public class MemberWithEmailAlreadyExistsException(string message) : Exception(message);