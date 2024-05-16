namespace Application.Exceptions.Cours;

public class CoursWithCodeAlreadyExistsException(string message) : Exception(message);