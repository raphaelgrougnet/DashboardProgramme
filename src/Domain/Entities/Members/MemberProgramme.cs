using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.Identity;
using Domain.Entities.Programmes;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Members;

[Index(nameof(Member) + "Id", nameof(Programme) + "Id", IsUnique = true)]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
public class MemberProgramme : AuditableEntity, IEquatable<MemberProgramme>
{
    public MemberProgramme()
    {
    }

    public MemberProgramme(Member member, Programme programme) : this()
    {
        Member = member;
        Programme = programme;
    }

    public Member Member { get; private set; } = default!;
    public Programme Programme { get; private set; } = default!;
    public User User => Member.User;

    public bool Equals(MemberProgramme? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Member.Equals(other.Member) && Programme.Equals(other.Programme);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((MemberProgramme)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Member, Programme);
    }

    public static bool operator ==(MemberProgramme? left, MemberProgramme? right)
    {
        return left != null && left.Equals(right);
    }

    public static bool operator !=(MemberProgramme? left, MemberProgramme? right)
    {
        return !(left == right);
    }
}