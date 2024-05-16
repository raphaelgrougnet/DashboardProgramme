﻿using Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Books;

public class Book : AuditableAndSoftDeletableEntity, ISanitizable
{
    public string NameFr { get; private set; } = default!;
    public string NameEn { get; private set; } = default!;
    public string DescriptionFr { get; private set; } = default!;
    public string DescriptionEn { get; private set; } = default!;

    [Precision(18, 2)] public decimal Price { get; set; }

    public string Isbn { get; private set; } = default!;
    public string Author { get; private set; } = default!;
    public string Editor { get; private set; } = default!;
    public string CardImage { get; private set; } = default!;
    public int YearOfPublication { get; private set; }
    public int NumberOfPages { get; private set; }
    public string Slug { get; private set; } = default!;

    public void SanitizeForSaving()
    {
        Isbn = Isbn.Trim();
        Author = Author.Trim();
        Editor = Editor.Trim();
        NameFr = NameFr.Trim();
        NameEn = NameEn.Trim();
        DescriptionFr = DescriptionFr.Trim();
        DescriptionEn = DescriptionEn.Trim();
    }

    public void SetAuthor(string author)
    {
        Author = author;
    }

    public void SetCardImageUrl(string cardImageUrl)
    {
        CardImage = cardImageUrl;
    }

    public void SetDescription(TranslatableString name)
    {
        DescriptionFr = name.Fr;
        DescriptionEn = name.En;
    }

    public void SetEditor(string editor)
    {
        Editor = editor;
    }

    public void SetIsbn(string isbn)
    {
        Isbn = isbn;
    }

    public void SetName(TranslatableString name)
    {
        NameFr = name.Fr;
        NameEn = name.En;
    }

    public void SetNumberOfPages(int numberOfPages)
    {
        NumberOfPages = numberOfPages;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
    }

    public void SetSlug(string value)
    {
        Slug = value;
    }

    public void SetYearOfPublication(int yearOfPublication)
    {
        YearOfPublication = yearOfPublication;
    }
}