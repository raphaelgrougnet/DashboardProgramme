using Application.Exceptions.IdentityError;
using Application.Interfaces.Services.Members;

using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Identity;

namespace Application.Services.Members;

public class MemberUpdateService(
    IMemberRepository memberRepository,
    IMemberProgrammeRepository
        memberProgrammeRepository,
    UserManager<User> userManager) :
    IMemberUpdateService
{
    public async Task UpdateMember(Guid id, string firstName, string lastName, string email, string? password, string?
        role, IEnumerable<Guid>? programmes)
    {
        Member member = memberRepository.FindById(id);

        member.SetFirstName(firstName);
        member.SetLastName(lastName);
        member.User.Email = email;

        if (password is not null)
        {
            IdentityResult? result = await userManager.RemovePasswordAsync(member.User);
            if (!result.Succeeded)
            {
                throw new AggregateException(result.Errors.Select(x => x.ToException()));
            }

            result = await userManager.AddPasswordAsync(member.User, password.Trim());
            if (!result.Succeeded)
            {
                throw new AggregateException(result.Errors.Select(x => x.ToException()));
            }
        }

        await memberRepository.ClearRoles(member);

        if (role is not null)
        {
            await memberRepository.AddRoleToMember(member, role);
        }

        if (programmes is not null)
        {
            HashSet<Guid> programmesDuMembre = member.MemberProgrammes.Select(p => p.Programme.Id).ToHashSet();
            IEnumerable<Guid> nouvLstProgrammes = programmes as Guid[] ?? programmes.ToArray();

            foreach (Guid programmeId in nouvLstProgrammes.Where(programmeId =>
                         !programmesDuMembre.Contains(programmeId)))
            {
                await memberProgrammeRepository.AddProgrammeToMember(member, programmeId);
            }

            foreach (Guid programmeId in programmesDuMembre.Where(programmeId =>
                         !nouvLstProgrammes.Contains(programmeId)))
            {
                await memberProgrammeRepository.RemoveProgrammeFromMember(member, programmeId);
            }
        }

        await memberRepository.UpdateMember(member);
    }
}