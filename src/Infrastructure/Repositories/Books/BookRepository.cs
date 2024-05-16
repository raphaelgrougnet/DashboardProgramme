using Application.Exceptions.Books;
using Application.Interfaces;

using Domain.Entities.Books;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;

using Slugify;

namespace Infrastructure.Repositories.Books;

public class BookRepository(IDashboardProgrammeDbContext context, ISlugHelper slugHelper)
    : IBookRepository
{
    public async Task CreateBook(Book book)
    {
        if (context.Books.Any(x => x.Isbn.Trim() == book.Isbn.Trim()))
        {
            throw new BookWithIsbnAlreadyExistsException($"A book with isbn {book.Isbn} already exists.");
        }

        GenerateSlug(book);
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBookWithId(Guid id)
    {
        Book? book = context.Books.FirstOrDefault(x => x.Id == id);
        if (book == null)
        {
            throw new BookNotFoundException($"Could not find book with id {id}.");
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }

    public Book FindById(Guid id)
    {
        Book? book = context.Books
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (book == null)
        {
            throw new BookNotFoundException($"Could not find book with id {id}.");
        }

        return book;
    }

    public List<Book> GetAll()
    {
        return context.Books.AsNoTracking().ToList();
    }

    public async Task UpdateBook(Book book)
    {
        if (context.Books.Any(x => x.Isbn == book.Isbn.Trim() && x.Id != book.Id))
        {
            throw new BookWithIsbnAlreadyExistsException($"Another book with isbn {book.Isbn} already exists.");
        }

        if (string.IsNullOrWhiteSpace(book.Slug))
        {
            GenerateSlug(book);
        }

        context.Books.Update(book);
        await context.SaveChangesAsync();
    }

    private void GenerateSlug(Book book)
    {
        string slug = book.NameFr;
        List<Book> slugs = context.Books.AsNoTracking().Where(x => x.Slug == slug).ToList();
        if (slugs.Count != 0)
        {
            slug = $"{slug}-{slug.Length + 1}";
        }

        book.SetSlug(slugHelper.GenerateSlug(slug).Replace(".", ""));
    }
}