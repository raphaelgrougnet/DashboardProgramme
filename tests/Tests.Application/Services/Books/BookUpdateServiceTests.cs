using Application.Exceptions.Books;
using Application.Interfaces.FileStorage;
using Application.Services.Books;

using Domain.Entities.Books;
using Domain.Repositories;

using Microsoft.AspNetCore.Http;

using Tests.Common.Builders;

namespace Tests.Application.Services.Books;

public class BookUpdateServiceTests
{
    private const string EXISTING_CARD_IMAGE_URL = "www.google.com";
    private const string NEW_CARD_IMAGE_URL = "blob.azure.com";

    private readonly IFormFile _anyCardImageFormFile;

    private readonly BookBuilder _bookBuilder;

    private readonly Mock<IBookRepository> _bookRepository;

    private readonly BookUpdateService _bookUpdateService;
    private readonly Mock<IFileStorageApiConsumer> _fileStorageApiConsumer;

    public BookUpdateServiceTests()
    {
        _anyCardImageFormFile = new FormFile(new MemoryStream(new byte[5]), long.MaxValue,
            long.MaxValue, "card image name", "any_filename");

        _bookBuilder = new BookBuilder();

        _bookRepository = new Mock<IBookRepository>();
        _fileStorageApiConsumer = new Mock<IFileStorageApiConsumer>();

        _bookUpdateService = new BookUpdateService(
            _bookRepository.Object,
            _fileStorageApiConsumer.Object);
    }

    private void GivenBookWithIdExists(Book book)
    {
        _bookRepository
            .Setup(x => x.FindById(It.IsAny<Guid>()))
            .Returns(book);
    }

    [Fact]
    public async Task GivenCardImageIsNotNull_WhenUpdateBook_ThenBookIsUpdatedWithNewCardImageUrl()
    {
        // Arrange
        Book updatedBook = null;
        Book book = _bookBuilder.WithCardImage(EXISTING_CARD_IMAGE_URL).Build();
        GivenBookWithIdExists(book);
        _fileStorageApiConsumer.Setup(x => x.UploadFileAsync(_anyCardImageFormFile)).ReturnsAsync(NEW_CARD_IMAGE_URL);
        _bookRepository.Setup(x => x.UpdateBook(It.IsAny<Book>())).Callback<Book>(bookParam => updatedBook = bookParam);

        // Act
        await _bookUpdateService.UpdateBook(book, _anyCardImageFormFile);

        // Assert
        updatedBook.CardImage.ShouldBe(NEW_CARD_IMAGE_URL);
    }

    [Fact]
    public async Task GivenCardImageIsNotNull_WhenUpdateBook_ThenDeleteExistingBookCardImageFromFileStorage()
    {
        // Arrange
        Book book = _bookBuilder.WithCardImage(EXISTING_CARD_IMAGE_URL).Build();
        GivenBookWithIdExists(book);

        // Act
        await _bookUpdateService.UpdateBook(book, _anyCardImageFormFile);

        // Assert
        _fileStorageApiConsumer.Verify(x => x.DeleteFileWithUrl(EXISTING_CARD_IMAGE_URL));
    }

    [Fact]
    public async Task GivenCardImageIsNotNull_WhenUpdateBook_ThenUploadNewCardImageToFileStorage()
    {
        // Arrange
        Book book = _bookBuilder.WithCardImage(EXISTING_CARD_IMAGE_URL).Build();
        GivenBookWithIdExists(book);

        // Act
        await _bookUpdateService.UpdateBook(book, _anyCardImageFormFile);

        // Assert
        _fileStorageApiConsumer.Verify(x => x.UploadFileAsync(_anyCardImageFormFile));
    }

    [Fact]
    public async Task GivenCardImageIsNull_WhenUpdateBook_ThenUseExistingBookCardImageForBookToUpdate()
    {
        // Arrange
        Book updatedBook = null;
        Book existingBook = _bookBuilder.WithCardImage(EXISTING_CARD_IMAGE_URL).Build();
        GivenBookWithIdExists(existingBook);
        _bookRepository.Setup(x => x.UpdateBook(It.IsAny<Book>())).Callback<Book>(bookParam => updatedBook = bookParam);
        Book book = _bookBuilder.Build();

        // Act
        await _bookUpdateService.UpdateBook(book, null);

        // Assert
        updatedBook.CardImage.ShouldBe(EXISTING_CARD_IMAGE_URL);
    }

    [Fact]
    public async Task GivenNoBookWithIdExists_WhenUpdateBook_ThenThrowBookNotFoundException()
    {
        // Arrange
        Book book = _bookBuilder.Build();

        // Act & assert
        await Assert.ThrowsAsync<BookNotFoundException>(async () =>
            await _bookUpdateService.UpdateBook(book, _anyCardImageFormFile));
    }

    [Fact]
    public async Task WhenUpdateBook_ThenDelegateUpdatingBookToBookRepository()
    {
        // Arrange
        Book book = _bookBuilder.Build();
        GivenBookWithIdExists(book);

        // Act
        await _bookUpdateService.UpdateBook(book, null);

        // Assert
        _bookRepository.Setup(x => x.UpdateBook(It.IsAny<Book>()));
    }
}