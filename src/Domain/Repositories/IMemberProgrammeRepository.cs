using Domain.Entities;
using Domain.Entities.Programmes;

namespace Domain.Repositories;

public interface IMemberProgrammeRepository
{
    IList<Programme> GetProgrammesByMemberId(Guid memberId);

    Task AddProgrammeToMember(Member member, Guid programmeId);
    Task RemoveProgrammeFromMember(Member member, Guid programmeId);
}