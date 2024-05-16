using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.Identity;
using Domain.Entities.Members;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.Entities;

[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class Member : AuditableAndSoftDeletableEntity, ISanitizable
{
    public Member()
    {
        MemberProgrammes = [];
    }

    public Member(string firstName, string lastName, int? apartment = null,
        string? street = null, string? city = null, string? zipCode = null) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Apartment = apartment;
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string Email => User.Email;

    public PhoneNumber? PhoneNumber => string.IsNullOrWhiteSpace(User.PhoneNumber)
        ? null
        : new PhoneNumber(User.PhoneNumber, User.PhoneExtension);

    public int? Apartment { get; private set; }
    public string? Street { get; private set; }
    public string? City { get; private set; }
    public string? ZipCode { get; private set; }
    public User User { get; private set; } = default!;

    public bool Active { get; private set; } = default!;

    public List<MemberProgramme> MemberProgrammes { get; set; } = default!;

    public void SanitizeForSaving()
    {
        FirstName = FirstName.Trim().CapitalizeFirstLetterOfEachWord()!;
        LastName = LastName.Trim().CapitalizeFirstLetterOfEachWord()!;
        Street = Street?.Trim().CapitalizeFirstLetterOfEachWord();
        City = City?.Trim().CapitalizeFirstLetterOfEachWord();
        ZipCode = ZipCode?.Trim().ToUpper();
    }

    public void Activate()
    {
        Restore();
        Active = true;
        User.Activate(FirstName);
    }

    public void AddProgramme(MemberProgramme memberProgramme)
    {
        MemberProgrammes.Add(memberProgramme);
    }

    public void Deactivate(string? deletedBy = null)
    {
        Active = false;
        User.SoftDelete(deletedBy);
    }

    public int? GetPhoneExtension()
    {
        return PhoneNumber?.Extension ?? null;
    }

    public string? GetPhoneNumber()
    {
        return PhoneNumber?.Number ?? null;
    }

    public void OnCreated(User user)
    {
        SetUser(user);
    }

    public void RemoveProgramme(MemberProgramme memberProgramme)
    {
        MemberProgrammes.Remove(memberProgramme);
    }

    public void SetApartment(int? apartment)
    {
        Apartment = apartment;
    }

    public void SetCity(string? city)
    {
        City = city;
    }

    public void SetFirstName(string firstName)
    {
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }

    public void SetStreet(string? street)
    {
        Street = street;
    }

    public void SetUser(User user)
    {
        User = user;
    }

    public void SetZipCode(string? zipCode)
    {
        ZipCode = zipCode;
    }

    public override void SoftDelete(string? deletedBy = null)
    {
        base.SoftDelete(deletedBy);
        Active = false;
        User.SoftDelete(deletedBy);
    }
}