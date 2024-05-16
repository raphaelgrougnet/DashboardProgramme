namespace Application.Exceptions.Books;

public class BookWithIsbnAlreadyExistsException(string message) : Exception(message);