using Application.Interfaces.FileStorage;
using Application.Interfaces.Services.Books;

using Domain.Entities.Books;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;

namespace Application.Services.Books;

public class BookCreationService(
    IBookRepository bookRepository,
    IFileStorageApiConsumer fileStorageApiConsumer)
    : IBookCreationService
{
    public async Task CreateBook(Book book, IFormFile cardImage)
    {
        book.SetCardImageUrl(await fileStorageApiConsumer.UploadFileAsync(cardImage));

        await bookRepository.CreateBook(book);
    }
}