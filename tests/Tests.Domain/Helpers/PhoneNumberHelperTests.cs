using Domain.Helpers;

namespace Tests.Domain.Helpers;

public class PhoneNumberHelperTests
{
    private const string ANY_PHONE_NUMBER = "555-555-5555";
    private const int ANY_PHONE_EXTENSION = 66;

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespacePhoneNumber_WhenAddExtensionToPhoneNumber_ThenReturnPhoneNumber(
        string? phoneNumber)
    {
        // Act
        string? phoneNumberWithExtension =
            PhoneNumberHelper.AddExtensionToPhoneNumber(phoneNumber, ANY_PHONE_EXTENSION);

        // Assert
        phoneNumberWithExtension.ShouldBe(phoneNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespacePhoneNumber_WhenFindExtensionInPhoneNumber_ThenReturnNull(string? phoneNumber)
    {
        // Act
        int? extension = PhoneNumberHelper.FindExtensionInPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespacePhoneNumber_WhenRemoveExtensionFromPhoneNumber_ThenReturnNull(
        string? phoneNumber)
    {
        // Act
        string? extension = PhoneNumberHelper.RemoveExtensionFromPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBeNull();
    }

    [Fact]
    public void GivenNullExtension_WhenAddExtensionToPhoneNumber_ThenReturnPhoneNumber()
    {
        // Act
        string? phoneNumberWithExtension = PhoneNumberHelper.AddExtensionToPhoneNumber(ANY_PHONE_NUMBER, null);

        // Assert
        phoneNumberWithExtension.ShouldBe(ANY_PHONE_NUMBER);
    }

    [Theory]
    [InlineData("555-555-5555 poste 12")]
    [InlineData("1-888-495-3948  poste 12  ")]
    public void GivenPhoneNumberWithExtension_WhenFindExtensionInPhoneNumber_ThenReturnExtension(string phoneNumber)
    {
        // Act
        int? extension = PhoneNumberHelper.FindExtensionInPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBe(12);
    }

    [Theory]
    [InlineData("555-555-5555 poste 12")]
    [InlineData("555-555-5555  poste 14  ")]
    public void GivenPhoneNumberWithExtension_WhenRemoveExtensionFromPhoneNumber_ThenReturnExtension(string phoneNumber)
    {
        // Act
        string? extension = PhoneNumberHelper.RemoveExtensionFromPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBe("555-555-5555");
    }

    [Theory]
    [InlineData("555-555-5555 poste ")]
    [InlineData("1-888-495-3948 poste")]
    public void GivenPhoneNumberWithoutExtensionNumber_WhenFindExtensionInPhoneNumber_ThenReturnNull(string phoneNumber)
    {
        // Act
        int? extension = PhoneNumberHelper.FindExtensionInPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBeNull();
    }

    [Theory]
    [InlineData("1-888-495-3948 poste ")]
    [InlineData("1-888-495-3948 poste")]
    public void
        GivenPhoneNumberWithoutExtensionNumber_WhenRemoveExtensionFromPhoneNumber_ThenReturnPhoneNumberWithoutSeparator(
            string phoneNumber)
    {
        // Act
        string? extension = PhoneNumberHelper.RemoveExtensionFromPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBe("1-888-495-3948");
    }

    [Theory]
    [InlineData("555-555-5555")]
    [InlineData("1-888-495-3948 p.")]
    public void GivenPhoneNumberWithoutExtensionSeparator_WhenFindExtensionInPhoneNumber_ThenReturnNull(
        string phoneNumber)
    {
        // Act
        int? extension = PhoneNumberHelper.FindExtensionInPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBeNull();
    }

    [Theory]
    [InlineData("555-555-5555")]
    [InlineData("1-888-495-3948 p.")]
    public void GivenPhoneNumberWithoutExtensionSeparator_WhenRemoveExtensionFromPhoneNumber_ThenReturnPhoneNumber(
        string phoneNumber)
    {
        // Act
        string? extension = PhoneNumberHelper.RemoveExtensionFromPhoneNumber(phoneNumber);

        // Assert
        extension.ShouldBe(phoneNumber);
    }

    [Fact]
    public void WhenAddExtensionToPhoneNumber_ThenReturnPhoneNumberAndExtensionSeparatedWithSeparator()
    {
        // Act
        string? phoneNumberWithExtension =
            PhoneNumberHelper.AddExtensionToPhoneNumber(ANY_PHONE_NUMBER, ANY_PHONE_EXTENSION);

        // Assert
        phoneNumberWithExtension.ShouldBe(
            $"{ANY_PHONE_NUMBER} {PhoneNumberHelper.EXTENSION_SEPARATOR} {ANY_PHONE_EXTENSION}");
    }
}