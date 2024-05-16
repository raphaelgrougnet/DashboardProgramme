using Application.Mapping.Profiles;

using AutoMapper;

using Domain.Constants.User;
using Domain.Entities;
using Domain.Entities.Identity;

using Tests.Application.TestCollections;
using Tests.Common.Builders;
using Tests.Common.Fixtures;
using Tests.Common.Mapping;

namespace Tests.Application.Mapping.Profiles;

[Collection(ApplicationTestCollection.NAME)]
public class MemberMappingProfileTests(TestFixture testFixture)
{
    private const string ANY_FIRST_NAME = "john";
    private const string ANY_LAST_NAME = "doe";
    private const string ANY_EMAIL = "john.doe@gmail.com";
    private const string ANY_PHONE_NUMBER = "514-640-8920";
    private const int ANY_PHONE_EXTENSION = 57;
    private const int ANY_APARTMENT = 3;
    private const string ANY_STREET = "965, Hollywood Blvd";
    private const string ANY_CITY = "Hollywood";
    private const string ANY_ZIP_CODE = "G7E 3L8";

    private readonly IMapper _mapper = new MapperBuilder().WithProfile<MemberMappingProfile>().Build();
    private readonly MemberBuilder _memberBuilder = new();

    private readonly UserBuilder _userBuilder = new();

    [Fact]
    public void GivenMember_WhenMap_ThenReturnUserMappedCorrectly()
    {
        // Arrange
        User memberUser = _userBuilder.WithRole(testFixture.FindRoleWithName(Roles.ADMINISTRATOR)).Build();
        Member member = _memberBuilder
            .WithFirstName(ANY_FIRST_NAME)
            .WithLastName(ANY_LAST_NAME)
            .WithEmail(ANY_EMAIL)
            .WithPhoneNumber(ANY_PHONE_NUMBER, ANY_PHONE_EXTENSION)
            .WithApartment(ANY_APARTMENT)
            .WithStreet(ANY_STREET)
            .WithCity(ANY_CITY)
            .WithZipCode(ANY_ZIP_CODE)
            .WithUser(memberUser)
            .Build();

        // Act
        User user = _mapper.Map<User>(member);

        // Assert
        user.Id.ShouldBe(member.User.Id);
        user.Email.ShouldBe(ANY_EMAIL.ToLower());
        user.UserName.ShouldBe(ANY_EMAIL.ToLower());
        user.NormalizedEmail.ShouldBe(ANY_EMAIL.Normalize());
        user.NormalizedUserName.ShouldBe(ANY_EMAIL.Normalize());
        user.PhoneNumber.ShouldBe(ANY_PHONE_NUMBER);
        user.PhoneExtension.ShouldBe(ANY_PHONE_EXTENSION);
        user.EmailConfirmed.ShouldBeTrue();
        user.PhoneNumberConfirmed.ShouldBeTrue();
        user.TwoFactorEnabled.ShouldBeTrue();
    }
}