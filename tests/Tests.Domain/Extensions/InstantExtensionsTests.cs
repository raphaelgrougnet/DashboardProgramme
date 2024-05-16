using Domain.Extensions;

using NodaTime;

namespace Tests.Domain.Extensions;

public class InstantExtensionsTests
{
    private const int EXPECTED_YEAR = 2022;
    private const int EXPECTED_MONTH = 12;
    private const int EXPECTED_DAY = 10;
    private const int EXPECTED_HOUR = 8;
    private const int EXPECTED_MINUTE = 5;
    private const int EXPECTED_SECOND = 45;

    [Fact]
    public void WhenFormatAsString_ThenReturnInstantFormattedWithYearMonthAndDaySeparedByHyphens()
    {
        // Arrange
        DateTime dateTime = new(EXPECTED_YEAR, EXPECTED_MONTH, EXPECTED_DAY, EXPECTED_HOUR, EXPECTED_MINUTE,
            EXPECTED_SECOND);
        Instant instant = dateTime.ParseToInstant();

        // Act
        string formattedInstant = instant.FormatAsString();

        // Assert
        formattedInstant.ShouldBe($"{EXPECTED_YEAR}-{EXPECTED_MONTH:00}-{EXPECTED_DAY:00}");
    }

    [Fact]
    public void WhenFormatAsStringWithTime_ThenReturnInstantCorrectlyFormatted()
    {
        // Arrange
        DateTime dateTime = new(EXPECTED_YEAR, EXPECTED_MONTH, EXPECTED_DAY, EXPECTED_HOUR, EXPECTED_MINUTE,
            EXPECTED_SECOND);
        Instant instant = dateTime.ParseToInstant();

        // Act
        string formattedInstant = instant.FormatAsStringWithTime();

        // Assert
        formattedInstant.ShouldBe(
            $"{EXPECTED_YEAR}-{EXPECTED_MONTH:00}-{EXPECTED_DAY:00} {EXPECTED_HOUR:00}:{EXPECTED_MINUTE:00}");
    }
}