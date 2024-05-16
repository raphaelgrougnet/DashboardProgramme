using Application.Services.Notifications.Dtos;

namespace Tests.Application.Services.Notifications.Dtos;

public class MemberDtoTests
{
    private const string ANY_FIRST_NAME = "john";
    private const string ANY_LAST_NAME = "doe";
    private const string ANY_EMAIL = "john.doe@gmail.com";
    private const string ANY_PHONE_NUMBER = "514-640-8920";
    private const int ANY_APARTMENT = 2;
    private const string ANY_STREET = "965, Hollywood Blvd";
    private const string ANY_CITY = "Hollywood";
    private const string ANY_ZIP_CODE = "G7E 3L8";

    [Fact]
    public void GivenAnyApartment_WhenNewMemberDto_ThenApartmentShouldBeSameAsGivenApartment()
    {
        // Act
        PersonDto memberDto = new() { Apartment = ANY_APARTMENT };

        // Assert
        memberDto.Apartment.ShouldBe(ANY_APARTMENT);
    }

    [Fact]
    public void GivenAnyCity_WhenNewMemberDto_ThenCityShouldBeSameAsGivenCity()
    {
        // Act
        PersonDto memberDto = new() { City = ANY_CITY };

        // Assert
        memberDto.City.ShouldBe(ANY_CITY);
    }

    [Fact]
    public void GivenAnyEmail_WhenNewMemberDto_ThenEmailAddressShouldBeSameAsGivenEmailAddress()
    {
        // Act
        PersonDto memberDto = new() { Email = ANY_EMAIL };

        // Assert
        memberDto.Email.ShouldBe(ANY_EMAIL);
    }

    [Fact]
    public void GivenAnyFirstName_WhenNewMemberDto_ThenFirstNameShouldBeSameAsGivenFirstName()
    {
        // Act
        PersonDto memberDto = new() { FirstName = ANY_FIRST_NAME };

        // Assert
        memberDto.FirstName.ShouldBe(ANY_FIRST_NAME);
    }

    [Fact]
    public void GivenAnyLastName_WhenNewMemberDto_ThenLastNameShouldBeSameAsGivenLastName()
    {
        // Act
        PersonDto memberDto = new() { LastName = ANY_LAST_NAME };

        // Assert
        memberDto.LastName.ShouldBe(ANY_LAST_NAME);
    }

    [Fact]
    public void GivenAnyPhoneNumber_WhenNewMemberDto_ThenPhoneNumberShouldBeSameAsGivenPhoneNumber()
    {
        // Act
        PersonDto memberDto = new() { PhoneNumber = ANY_PHONE_NUMBER };

        // Assert
        memberDto.PhoneNumber.ShouldBe(ANY_PHONE_NUMBER);
    }

    [Fact]
    public void GivenAnyStreet_WhenNewMemberDto_ThenStreetShouldBeSameAsGivenStreet()
    {
        // Act
        PersonDto memberDto = new() { Street = ANY_STREET };

        // Assert
        memberDto.Street.ShouldBe(ANY_STREET);
    }

    [Fact]
    public void GivenAnyZipCode_WhenNewMemberDto_ThenZipCodeShouldBeSameAsGivenZipCode()
    {
        // Act
        PersonDto memberDto = new() { ZipCode = ANY_ZIP_CODE };

        // Assert
        memberDto.ZipCode.ShouldBe(ANY_ZIP_CODE);
    }
}