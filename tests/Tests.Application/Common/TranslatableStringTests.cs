using System.Globalization;

using Application.Helpers;

using Domain.Common;

namespace Tests.Application.Common;

public class TranslatableStringTests
{
    [Fact]
    public void GivenDefinedFrAndEnAndPartiallyIsTrue_WhenIsEmpty_ThenReturnFalse()
    {
        // Arrange
        TranslatableString field = new("Ceci est du contenu", "This is some content");

        // Act & assert
        field.IsEmpty(true).ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(" ", " ")]
    [InlineData(" \n   ", "   \t")]
    [InlineData(null, "")]
    [InlineData("", null)]
    public void GivenEmptyFrAndEn_WhenIsEmpty_ThenReturnTrue(string fr, string en)
    {
        // Arrange
        TranslatableString field = new() { Fr = fr, En = en };

        // Act & assert
        field.IsEmpty().ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, "Hey there!")]
    [InlineData("C'est pas toi", " ")]
    [InlineData(" \n   ", " oh well")]
    [InlineData("et bien non...", "")]
    public void GivenEmptyFrOrEnAndPartiallyIsTrue_WhenIsEmpty_ThenReturnTrue(string fr, string en)
    {
        // Arrange
        TranslatableString field = new() { Fr = fr, En = en };

        // Act & assert
        field.IsEmpty(true).ShouldBeTrue();
    }

    [Theory]
    [InlineData("Ceci est du contenu", "Ceci est du contenu", "This is some content", "This is some awesome content")]
    [InlineData("Ceci est du contenu", "Ceci est du contenu super", "This is some content", "This is some content")]
    [InlineData("Ceci est du contenu", "Ceci est du contenu super", "This is some content",
        "This is awesome some content")]
    public void GivenFrAndEnAreSetAndDifferent_WhenEquals_ThenReturnFalse(string fr1, string fr2, string en1,
        string en2)
    {
        // Arrange
        TranslatableString field = new(fr1, en1);
        TranslatableString field2 = new(fr2, en2);

        // Act & assert
        field.Equals(field2).ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, "Hey there!")]
    [InlineData("C'est pas toi", " ")]
    [InlineData(" \n   ", " oh well")]
    [InlineData("et bien non...", "")]
    [InlineData("J'ai une valeur", "I do too")]
    public void GivenFrOrEnIsNotEmpty_WhenIsEmpty_ThenReturnFalse(string fr, string en)
    {
        // Arrange
        TranslatableString field = new() { Fr = fr, En = en };

        // Act & assert
        field.IsEmpty().ShouldBeFalse();
    }

    [Fact]
    public void GivenNoLanguageAndUnsupportedCurrentCulture_WhenTransl8_ThenThrowException()
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
        TranslatableString field = new() { Fr = "Bonjour", En = "Hi" };

        // Act & assert
        Exception exception = Assert.Throws<Exception>(() => field.Transl8());
        exception.Message.ShouldStartWith(
            "Could not get translation. Language \"es\" (current UI culture) is not managed.");
    }

    [Theory]
    [InlineData("en", "Welcome")]
    [InlineData("EN", "Welcome")]
    [InlineData("fr", "Bienvenue")]
    [InlineData("FR", "Bienvenue")]
    public void GivenTwoLetterLang_WhenToString_ThenReturnTranslationForSpecifiedLanguage(
        string language,
        string expected)
    {
        // Arrange
        TranslatableString field = new() { Fr = "Bienvenue", En = "Welcome" };
        Thread.CurrentThread.CurrentUICulture = CultureHelper.ConvertTwoLetterIsoToCultureInfo(language);

        // Act
        string translation = field.ToString();

        // Assert
        translation.ShouldBe(expected);
    }

    [Theory]
    [InlineData("en", "Welcome")]
    [InlineData("EN", "Welcome")]
    [InlineData("fr", "Bienvenue")]
    [InlineData("FR", "Bienvenue")]
    public void GivenTwoLetterLang_WhenTransl8_ThenReturnTranslationForSpecifiedLanguage(
        string language,
        string expected)
    {
        // Arrange
        TranslatableString field = new() { Fr = "Bienvenue", En = "Welcome" };

        // Act
        string translation = field.Transl8(language);

        // Assert
        translation.ShouldBe(expected);
    }

    [Fact]
    public void GivenUnsupportedLanguage_WhenTransl8_ThenThrowException()
    {
        // Arrange
        TranslatableString field = new() { Fr = "Bonjour", En = "Hi" };

        // Act & assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => field.Transl8("es"));
        exception.Message.ShouldStartWith("Could not get translation. Language \"es\" is not managed.");
    }

    [Theory]
    [InlineData("en-CA", "Welcome")]
    [InlineData("fr-CA", "Bienvenue")]
    public void GivenWhitespaceLanguage_WhenTransl8_ThenReturnTranslationForCurrentUICulture(
        string culture,
        string expected)
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        TranslatableString field = new() { Fr = "Bienvenue", En = "Welcome" };

        // Act
        string translation = field.Transl8(" ");

        // Assert
        translation.ShouldBe(expected);
    }

    [Theory]
    [InlineData("en-CA", "Welcome")]
    [InlineData("fr-CA", "Bienvenue")]
    public void WhenTransl8_ShouldReturnTranslationForCurrentUICulture_WhenLanguageIsNotGiven(
        string culture,
        string expected)
    {
        // Arrange
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        TranslatableString field = new() { Fr = "Bienvenue", En = "Welcome" };

        // Act
        string translation = field.Transl8();

        // Assert
        translation.ShouldBe(expected);
    }
}