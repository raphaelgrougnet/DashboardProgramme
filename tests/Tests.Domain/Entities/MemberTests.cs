using Domain.Entities;
using Domain.Entities.Identity;

using NodaTime;

using Tests.Common.Builders;

namespace Tests.Domain.Entities;

public class MemberTests
{
    private const string ANY_EMAIL = "garneau@spektrummedia.com";

    private const string FIRST_NAME = "  jane";
    private const string SANITIZED_FIRST_NAME = "Jane";
    private const string LAST_NAME = "blo  ";
    private const string SANITIZED_LAST_NAME = "Blo";
    private const string STREET = "111, HOLLYWOOD Blvd  ";
    private const string SANITIZED_STREET = "111, Hollywood Blvd";
    private const string CITY = "SAN Francisco  ";
    private const string SANITIZED_CITY = "San Francisco";
    private const string ZIP_CODE = "H1s 7k1  ";
    private const string SANITIZED_ZIP_CODE = "H1S 7K1";

    private readonly Member _member;

    private readonly UserBuilder _userBuilder;

    public MemberTests()
    {
        _userBuilder = new UserBuilder();
        MemberBuilder memberBuilder = new();

        User user = _userBuilder.WithEmail(ANY_EMAIL).Build();
        _member = memberBuilder.WithFirstName(FIRST_NAME).WithLastName(LAST_NAME).WithUser(user).Build();
    }

    [Fact]
    public void GivenNullCity_WhenSanitizeForSaving_ThenReturnNull()
    {
        // Arrange
        _member.SetCity(null);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.City.ShouldBeNull();
    }

    [Fact]
    public void GivenNullStreet_WhenSanitizeForSaving_ThenReturnNull()
    {
        // Arrange
        _member.SetStreet(null);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.Street.ShouldBeNull();
    }

    [Fact]
    public void GivenNullZipCode_WhenSanitizeForSaving_ThenReturnNull()
    {
        // Arrange
        _member.SetZipCode(null);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.ZipCode.ShouldBeNull();
    }

    [Fact]
    public void OnCreated_ThenSetUserAsMemberUser()
    {
        // Arrange
        User user = _userBuilder.Build();

        // Act
        _member.OnCreated(user);

        // Assert
        _member.User.ShouldBe(user);
    }

    [Fact]
    public void WhenActivate_ThenSetDeletedByToNull()
    {
        // Arrange
        _member.DeletedBy = ANY_EMAIL;

        // Act
        _member.Activate();

        // Assert
        _member.DeletedBy.ShouldBeNull();
    }

    [Fact]
    public void WhenActivate_ThenSetDeletedToNull()
    {
        // Arrange
        _member.Deleted = Instant.MaxValue;

        // Act
        _member.Activate();

        // Assert
        _member.Deleted.ShouldBeNull();
    }

    [Fact]
    public void WhenSanitizeForSaving_ThenSanitizeCity()
    {
        // Arrange
        _member.SetCity(CITY);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.City.ShouldBe(SANITIZED_CITY);
    }

    [Fact]
    public void WhenSanitizeForSaving_ThenSanitizeFirstName()
    {
        // Arrange
        _member.SetFirstName(FIRST_NAME);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.FirstName.ShouldBe(SANITIZED_FIRST_NAME);
    }

    [Fact]
    public void WhenSanitizeForSaving_ThenSanitizeLastName()
    {
        // Arrange
        _member.SetLastName(LAST_NAME);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.LastName.ShouldBe(SANITIZED_LAST_NAME);
    }

    [Fact]
    public void WhenSanitizeForSaving_ThenSanitizeStreet()
    {
        // Arrange
        _member.SetStreet(STREET);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.Street.ShouldBe(SANITIZED_STREET);
    }

    [Fact]
    public void WhenSanitizeForSaving_ThenSanitizeZipCode()
    {
        // Arrange
        _member.SetZipCode(ZIP_CODE);

        // Act
        _member.SanitizeForSaving();

        // Assert
        _member.ZipCode.ShouldBe(SANITIZED_ZIP_CODE);
    }

    [Fact]
    public void WhenSetUser_ThenMemberUserIsSameAsGivenUser()
    {
        // Arrange
        User user = _userBuilder.Build();

        // Act
        _member.SetUser(user);

        // Assert
        _member.User.ShouldBe(user);
    }
}