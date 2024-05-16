using FastEndpoints;

using FluentValidation;

namespace Web.Features.Admins.Books.DeleteBook;

public class DeleteBookValidator : Validator<DeleteBookRequest>
{
    public DeleteBookValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode("EmptyBookId")
            .WithMessage("Book id is required.");
    }
}