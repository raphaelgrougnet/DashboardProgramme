using Application.Exceptions.Books;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Services.Books;

using Domain.Entities.Books;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;

namespace Application.Services.Books;

public class BookUpdateService(
    IBookRepository bookRepository,
    IFileStorageApiConsumer fileStorageApiConsumer)
    : IBookUpdateService
{
    public async Task UpdateBook(Book book, IFormFile? cardImage)
    {
        Book? existingBook = bookRepository.FindById(book.Id);
        if (existingBook == null)
        {
            throw new BookNotFoundException($"Could not find book with id {book.Id}.");
        }

        book = await UpdateCardImage(existingBook, book, cardImage);

        await bookRepository.UpdateBook(book);
    }

    private async Task<Book> UpdateCardImage(Book existingBook, Book book, IFormFile? cardImage)
    {
        if (cardImage == null)
        {
            book.SetCardImageUrl(existingBook.CardImage);
            return book;
        }

        await fileStorageApiConsumer.DeleteFileWithUrl(existingBook.CardImage);
        book.SetCardImageUrl(await fileStorageApiConsumer.UploadFileAsync(cardImage));
        return book;
    }
}