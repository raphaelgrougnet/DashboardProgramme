using Domain.Entities;
using Domain.Entities.Identity;
using Domain.ValueObjects;

using NodaTime;

namespace Tests.Common.Builders;

public class MemberBuilder
{
    private const string ANY_FIRST_NAME = "john";
    private const string ANY_LAST_NAME = "doe";
    private const string ANY_EMAIL = "john.doe@gmail.com";
    private const string ANY_PHONE_NUMBER = "514-640-8920";
    private const int ANY_PHONE_EXTENSION = 93;
    private const string ANY_STREET = "965, Hollywood Blvd";
    private const string ANY_CITY = "Hollywood";
    private const string ANY_ZIP_CODE = "G7E 3L8";

    private Guid? Id { get; set; }
    private string? FirstName { get; set; }
    private string? LastName { get; set; }
    private string? Email { get; set; }
    private PhoneNumber? PhoneNumber { get; set; }
    private int? Apartment { get; set; }
    private string? Street { get; set; }
    private string? City { get; set; }
    private string? ZipCode { get; set; }
    private Instant? Deleted { get; }
    private string? DeletedBy { get; }
    private User? User { get; set; }
    private bool? Active { get; set; }

    public Member Build()
    {
        Member member = new(
            FirstName ?? ANY_FIRST_NAME,
            LastName ?? ANY_LAST_NAME,
            Apartment,
            Street ?? ANY_STREET,
            City ?? ANY_CITY,
            ZipCode ?? ANY_ZIP_CODE
        );

        User ??= new User();
        User.PhoneNumber = PhoneNumber?.Number ?? ANY_PHONE_NUMBER;
        User.PhoneExtension = PhoneNumber?.Extension ?? ANY_PHONE_EXTENSION;
        User.Email = Email ?? ANY_EMAIL;
        member.SetUser(User);

        if (Active.HasValue && Active.Value)
        {
            member.Activate();
        }

        if (Active.HasValue && Active.Value)
        {
            member.Activate();
        }

        if (DeletedBy != null || Deleted != null)
        {
            member.SoftDelete(DeletedBy);
        }

        member.SetId(Id ?? Guid.Empty);

        return member;
    }

    public MemberBuilder WithActive(bool active)
    {
        Active = active;
        return this;
    }

    public MemberBuilder WithApartment(int apartment)
    {
        Apartment = apartment;
        return this;
    }

    public MemberBuilder WithCity(string city)
    {
        City = city;
        return this;
    }

    public MemberBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public MemberBuilder WithFirstName(string firstName)
    {
        FirstName = firstName;
        return this;
    }

    public MemberBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public MemberBuilder WithLastName(string lastName)
    {
        LastName = lastName;
        return this;
    }

    public MemberBuilder WithPhoneNumber(string phoneNumber, int? extension = null)
    {
        PhoneNumber = new PhoneNumber(phoneNumber, extension);
        return this;
    }

    public MemberBuilder WithStreet(string street)
    {
        Street = street;
        return this;
    }

    public MemberBuilder WithUser(User user)
    {
        User = user;
        return this;
    }

    public MemberBuilder WithZipCode(string zipCode)
    {
        ZipCode = zipCode;
        return this;
    }
}