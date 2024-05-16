using Application.Extensions;

namespace Tests.Application.Extensions;

public class StringExtensionsTests
{
    private const string ANY_VALUE = "hello";
    private const string UTF8_PREFIX = "=?UTF-8?B?";
    private const string STRING_WITH_INT_VALUE = "34";
    private const int INT_VALUE_INSIDE_STRING = 34;

    [Fact]
    public void GivenStringWithNoIntInside_WhenIntTryParseOrZero_ThenReturnZero()
    {
        // Act
        int actualValue = ANY_VALUE.IntTryParseOrZero();

        // Assert
        actualValue.ShouldBe(0);
    }

    [Fact]
    public void GivenStringWithValidIntInside_WhenIntTryParseOrZero_ThenReturnIntThatWasInString()
    {
        // Act
        int actualValue = STRING_WITH_INT_VALUE.IntTryParseOrZero();

        // Assert
        actualValue.ShouldBe(INT_VALUE_INSIDE_STRING);
    }

    [Fact]
    public void GivenValidInt_WhenIntTryParseOrZero_ThenReturnCorrespondingInt()
    {
        // Arrange


        int actualValue = STRING_WITH_INT_VALUE.IntTryParseOrZero();

        // Assert
        actualValue.ShouldBe(34);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("MTIzNDU", "12345")]
    [InlineData("d2hhdF9hLXZlcnkrc3RyYW5nZUBlbWFpbC1hZGRyZXNzLmNvbQ", @"what_a-very+strange@email-address.com")]
    [InlineData("QHphXjYkTnozQ2svL1UlNVwtKytIdjM9NjlkYlo", @"@za^6$Nz3Ck//U%5\-++Hv3=69dbZ")]
    public void WhenBase64Decode_ThenReturnCorrectlyDecodedString(string clearText, string encoded)
    {
        // Arrange & act
        string result = clearText.Base64UrlDecode();

        // Assert
        result.ShouldBe(encoded);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("12345", "MTIzNDU")]
    [InlineData(@"what_a-very+strange@email-address.com", "d2hhdF9hLXZlcnkrc3RyYW5nZUBlbWFpbC1hZGRyZXNzLmNvbQ")]
    [InlineData(@"@za^6$Nz3Ck//U%5\-++Hv3=69dbZ", "QHphXjYkTnozQ2svL1UlNVwtKytIdjM9NjlkYlo")]
    public void WhenBase64Encode_ThenReturnCorrectlyEncodedString(string clearText, string encoded)
    {
        // Arrange & act
        string result = clearText.Base64UrlEncode();

        // Assert
        result.ShouldBe(encoded);
    }

    [Fact]
    public void WhenToRfc1342Base64_ThenReturnValueStartsWithUtf8()
    {
        string actual = ANY_VALUE.ToRfc1342Base64();

        actual.ShouldStartWith(UTF8_PREFIX);
    }
}