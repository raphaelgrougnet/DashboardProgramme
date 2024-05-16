using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Infrastructure.Repositories.Members;

using Microsoft.AspNetCore.Identity;

using Tests.Common.Fixtures;
using Tests.Infrastructure.TestCollections;

namespace Tests.Infrastructure.Repositories.Members;

[Collection(InfrastructureTestCollection.NAME)]
public class MemberRepositoryTests
{
    private readonly IMemberRepository _memberRepository;
    private readonly TestFixture _testFixture;

    public MemberRepositoryTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        UserManager<User> userManager = _testFixture.UserManager;
        _memberRepository = new MemberRepository(_testFixture.DbContext, userManager);
    }

    private async Task<Member> GivenDatabaseHasOrganizationMember()
    {
        User user = await _testFixture.GivenUserInDatabase();
        Member member = _testFixture.MemberBuilder.WithUser(user).Build();
        _testFixture.DbContext.Members.Add(member);
        await _testFixture.DbContext.SaveChangesAsync();
        return member;
    }

    [Fact]
    public async Task WhenFindByUserEmail_ThenReturnMemberAssociatedWithUser()
    {
        // Arrange
        Member expectedMember = await GivenDatabaseHasOrganizationMember();

        // Act
        Member? actualMember = _memberRepository.FindByUserEmail(expectedMember.User.Email);

        // Assert
        actualMember?.Email.ShouldBe(expectedMember.Email);
    }

    [Fact]
    public async Task WhenFindByUserId_ThenReturnMemberAssociatedWithUser()
    {
        // Arrange
        Member expectedMember = await GivenDatabaseHasOrganizationMember();

        // Act
        Member? actualMember = _memberRepository.FindByUserId(expectedMember.User.Id);

        // Assert
        actualMember?.Id.ShouldBe(expectedMember.Id);
    }

    [Fact]
    public async Task WhenGetContactCount_ThenReturnNumberOfMembersInDatabase()
    {
        // Arrange
        await GivenDatabaseHasOrganizationMember();
        int expectedCount = _testFixture.DbContext.Members.Count();

        // Act
        int contactCount = _memberRepository.GetMemberCount();

        // Assert
        contactCount.ShouldBe(expectedCount);
    }
}