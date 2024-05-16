using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Repositories;

namespace Application.Services.Members;

public class MemberCreationService(
    IMemberRepository memberRepository,
    IMemberProgrammeRepository memberProgrammeRepository) :
    IMemberCreationService
{
    public async Task CreateMember(string firstName, string lastName, string email, string password, string? role,
        IEnumerable<Guid>? programmes)
    {
        Member newMember = await memberRepository.CreateMember(firstName, lastName, email, password);

        if (role is not null)
        {
            await memberRepository.AddRoleToMember(newMember, role);
        }

        if (programmes is not null)
        {
            foreach (Guid programmeId in programmes)
            {
                await memberProgrammeRepository.AddProgrammeToMember(newMember, programmeId);
            }
        }
    }
}