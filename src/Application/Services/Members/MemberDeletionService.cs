using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Members;

public class MemberDeletionService(
    IMemberRepository memberRepository,
    IAuthenticatedMemberService authenticatedMemberService) :
    IMemberDeletionService
{
    public async Task DeleteMemberWithId(Guid id)
    {
        Member target = memberRepository.FindById(id);
        Member deleter = authenticatedMemberService.GetAuthenticatedMember();
        target.Deactivate(deleter.Email);
        await memberRepository.DeleteMemberWithId(id);
    }
}