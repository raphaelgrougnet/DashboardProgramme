namespace Application.Exceptions.Books;

public class BookNotFoundException(string message) : Exception(message);