using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.Programmes;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.GrilleProgrammes;

[Index(nameof(Programme) + "Id", nameof(EtaleeSurNbSessions), nameof(AnneeMiseAJour), IsUnique = true)]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public class GrilleProgramme : AuditableEntity, IEquatable<GrilleProgramme>, IComparable<GrilleProgramme>
{
    public GrilleProgramme()
    {
        GrilleProgrammeCours = [];
    }

    public GrilleProgramme(Programme programme, byte etaleeSurNbSessions, ushort anneeMiseAJour) : this()
    {
        Programme = programme;
        EtaleeSurNbSessions = etaleeSurNbSessions;
        AnneeMiseAJour = anneeMiseAJour;
    }


    public Programme Programme { get; private set; } = default!;
    public byte EtaleeSurNbSessions { get; private set; } = default!;
    public ushort AnneeMiseAJour { get; private set; } = default!;
    public List<GrilleProgrammeCours> GrilleProgrammeCours { get; private set; } = default!;

    public int CompareTo(GrilleProgramme? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return ReferenceEquals(null, other) ? 1 : AnneeMiseAJour.CompareTo(other.AnneeMiseAJour);
    }

    public bool Equals(GrilleProgramme? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (Programme.Equals(other.Programme) && EtaleeSurNbSessions == other.EtaleeSurNbSessions &&
                AnneeMiseAJour == other.AnneeMiseAJour) || (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void AddGrilleProgrammeCours(GrilleProgrammeCours grilleProgrammeCours)
    {
        GrilleProgrammeCours.Add(grilleProgrammeCours);
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

        return obj.GetType() == GetType() && Equals((GrilleProgramme)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Programme, EtaleeSurNbSessions, AnneeMiseAJour);
    }

    public static bool operator ==(GrilleProgramme? a, GrilleProgramme? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(GrilleProgramme? a, GrilleProgramme? b)
    {
        return !(a == b);
    }

    public void RemoveGrilleProgrammeCours(GrilleProgrammeCours grilleProgrammeCours)
    {
        GrilleProgrammeCours.Remove(grilleProgrammeCours);
    }
}