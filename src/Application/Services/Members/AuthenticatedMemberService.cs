using Application.Exceptions.Members;
using Application.Exceptions.Users;
using Application.Interfaces.Services.Members;
using Application.Interfaces.Services.Users;

using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

namespace Application.Services.Members;

public class AuthenticatedMemberService(
    IMemberRepository memberRepository,
    IAuthenticatedUserService authenticatedUserService)
    : IAuthenticatedMemberService
{
    public Member GetAuthenticatedMember()
    {
        User? user = authenticatedUserService.GetAuthenticatedUser();
        if (user == null)
        {
            throw new UserNotFoundException("Could not find user associated with authenticated member.");
        }

        Member? member = memberRepository.FindByUserId(user.Id);
        if (member == null)
        {
            throw new MemberNotFoundException($"Could not find member associated with user with id {user.Id}");
        }

        return member;
    }
}