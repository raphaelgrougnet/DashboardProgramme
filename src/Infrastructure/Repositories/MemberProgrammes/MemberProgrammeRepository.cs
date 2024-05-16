using System.Collections.Immutable;

using Application.Interfaces;

using Domain.Entities;
using Domain.Entities.Members;
using Domain.Entities.Programmes;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.MemberProgrammes;

public class MemberProgrammeRepository : IMemberProgrammeRepository
{
    private readonly IDashboardProgrammeDbContext _context;

    public MemberProgrammeRepository(IDashboardProgrammeDbContext context)
    {
        _context = context;
    }

    public async Task AddProgrammeToMember(Member member, Guid programmeId)
    {
        if (_context.MemberProgrammes.Any(mp => mp.Member.Id.Equals(member.Id) && mp.Programme.Id.Equals(programmeId)))
        {
            return;
        }

        MemberProgramme memberProgramme = new(member,
            await _context.Programmes.FindAsync(programmeId) ??
            throw new InvalidOperationException("Programme not found"));
        await _context.MemberProgrammes.AddAsync(memberProgramme);
        member.AddProgramme(memberProgramme);
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }

    public IList<Programme> GetProgrammesByMemberId(Guid memberId)
    {
        return _context.MemberProgrammes.AsNoTracking().Where(mp => mp.Member.Id.Equals(memberId))
            .Select(mp => mp.Programme).ToImmutableList();
    }

    public async Task RemoveProgrammeFromMember(Member member, Guid programmeId)
    {
        MemberProgramme? memberProgramme = _context.MemberProgrammes.FirstOrDefault(mp =>
            mp.Member.Id.Equals(member.Id) && mp.Programme.Id.Equals(programmeId));

        if (memberProgramme is null)
        {
            return;
        }

        _context.MemberProgrammes.Remove(memberProgramme);
        member.RemoveProgramme(memberProgramme);
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }
}