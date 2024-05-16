using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.CoursNs;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.GrilleProgrammes;

[Index(nameof(GrilleProgrammes.GrilleProgramme) + "Id", nameof(Cours) + "Id")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public class GrilleProgrammeCours : AuditableEntity, IEquatable<GrilleProgrammeCours>
{
    public GrilleProgrammeCours()
    {
    }

    public GrilleProgrammeCours(GrilleProgramme grilleProgramme, Cours cours, byte numeroSessionPrevu) : this()
    {
        GrilleProgramme = grilleProgramme;
        Cours = cours;
        NumeroSessionPrevu = numeroSessionPrevu;
    }

    public GrilleProgramme GrilleProgramme { get; private set; } = default!;
    public Cours Cours { get; private set; } = default!;
    public byte NumeroSessionPrevu { get; private set; } = default!;

    public bool Equals(GrilleProgrammeCours? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (GrilleProgramme.Equals(other.GrilleProgramme) && Cours.Equals(other.Cours)) || (Id != Guid.Empty && Id
            .Equals(other.Id));
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

        return obj.GetType() == GetType() && Equals((GrilleProgrammeCours)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GrilleProgramme, Cours);
    }

    public static bool operator ==(GrilleProgrammeCours? a, GrilleProgrammeCours? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(GrilleProgrammeCours? a, GrilleProgrammeCours? b)
    {
        return !(a == b);
    }
}