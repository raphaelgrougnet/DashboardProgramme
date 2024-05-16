﻿using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Books.GetBook;

public class GetBookValidator : Validator<GetBookRequest>
{
    public GetBookValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyBookId")
            .WithMessage("Book id is required.");
    }
}