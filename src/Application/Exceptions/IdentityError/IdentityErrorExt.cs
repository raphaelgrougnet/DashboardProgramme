namespace Application.Exceptions.IdentityError;

public static class IdentityErrorExt
{
    public static Exception ToException(this Microsoft.AspNetCore.Identity.IdentityError error)
    {
        return error.Code switch
        {
            "DuplicateEmail" => new DuplicateEmailException(error.Description),
            "DuplicateRoleName" => new DuplicateRoleNameException(error.Description),
            "InvalidEmail" => new InvalidEmailException(error.Description),
            "InvalidRoleName" => new InvalidRoleNameException(error.Description),
            "InvalidToken" => new InvalidTokenException(error.Description),
            "InvalidUserName" => new InvalidUserNameException(error.Description),
            "LoginAlreadyAssociated" => new LoginAlreadyAssociatedException(error.Description),
            "PasswordMismatch" => new PasswordMismatchException(error.Description),
            "PasswordRequiresDigit" => new PasswordRequiresDigitException(error.Description),
            "PasswordRequiresLower" => new PasswordRequiresLowerException(error.Description),
            "PasswordRequiresNonAlphanumeric" => new PasswordRequiresNonAlphanumericException(error.Description),
            "PasswordRequiresUniqueChars" => new PasswordRequiresUniqueCharsException(error.Description),
            "PasswordRequiresUpper" => new PasswordRequiresUpperException(error.Description),
            "PasswordTooShort" => new PasswordTooShortException(error.Description),
            "UserAlreadyHasPassword" => new UserAlreadyHasPasswordException(error.Description),
            "UserAlreadyInRole" => new UserAlreadyInRoleException(error.Description),
            "UserNotInRole" => new UserNotInRoleException(error.Description),
            _ => new Exception($"{error.Code}: {error.Description}")
        };
    }
}