using Application.Exceptions.Members;
using Application.Exceptions.Users;
using Application.Interfaces.Services.Users;
using Application.Services.Members;

using Domain.Constants.User;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Tests.Application.TestCollections;
using Tests.Common.Builders;
using Tests.Common.Fixtures;

namespace Tests.Application.Services.Members;

[Collection(ApplicationTestCollection.NAME)]
public class AuthenticatedMemberServiceTests
{
    private readonly AuthenticatedMemberService _authenticatedMemberService;
    private readonly Mock<IAuthenticatedUserService> _authenticatedUserService;
    private readonly MemberBuilder _memberBuilder;

    private readonly Mock<IMemberRepository> _memberRepository;
    private readonly TestFixture _testFixture;

    private readonly UserBuilder _userBuilder;

    public AuthenticatedMemberServiceTests(TestFixture testFixture)
    {
        _testFixture = testFixture;

        _userBuilder = new UserBuilder();
        _memberBuilder = new MemberBuilder();
        _memberRepository = new Mock<IMemberRepository>();
        _authenticatedUserService = new Mock<IAuthenticatedUserService>();

        _authenticatedMemberService = new AuthenticatedMemberService(_memberRepository.Object,
            _authenticatedUserService.Object);
    }

    private Member GivenAuthenticatedMemberWithUserIsFound()
    {
        User user = _userBuilder.WithRole(_testFixture.FindRoleWithName(Roles.ADMINISTRATOR)).Build();
        GivenAuthenticatedUserServiceReturnsUser(user);

        Member member = _memberBuilder.WithUser(user).Build();
        _memberRepository.Setup(x => x.FindByUserId(user.Id, true)).Returns(member);
        return member;
    }

    [Fact]
    public void
        GivenAuthenticatedUserServiceReturnsNull_WhenGetAuthenticatedMember_ThenThrowGetAuthenticatedMemberException()
    {
        // Act & assert
        Assert.Throws<UserNotFoundException>(() => _authenticatedMemberService.GetAuthenticatedMember());
    }

    private void GivenAuthenticatedUserServiceReturnsUser(User user)
    {
        _authenticatedUserService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
    }

    [Fact]
    public void
        GivenAuthenticatedUserServiceReturnsUser_WhenGetAuthenticatedMember_ThenReturnMemberLinkedToAuthenticatedUser()
    {
        // Arrange
        Member member = GivenAuthenticatedMemberWithUserIsFound();

        // Act
        Member authenticatedMember = _authenticatedMemberService.GetAuthenticatedMember();

        // Assert
        authenticatedMember.ShouldNotBeNull();
        authenticatedMember.User.Id.ShouldBe(member.User.Id);
    }

    [Fact]
    public void
        GivenMemberRepositoryDoesNotFindAnyMemberWithGivenUserId_WhenGetAuthenticatedMember_ThenThrowGetAuthenticatedMemberException()
    {
        // Arrange
        Role role = _testFixture.FindRoleWithName(Roles.ADMINISTRATOR);
        User user = _userBuilder.WithRole(role).Build();
        GivenAuthenticatedUserServiceReturnsUser(user);

        // Act & assert
        Assert.Throws<MemberNotFoundException>(() => _authenticatedMemberService.GetAuthenticatedMember());
    }
}