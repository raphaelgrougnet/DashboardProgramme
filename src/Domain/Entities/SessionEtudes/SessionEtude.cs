using System.Diagnostics.CodeAnalysis;

using Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.SessionEtudes;

[Index(nameof(Slug), IsUnique = true)]
[Index(nameof(Annee), nameof(Saison), IsUnique = true)]
[Index(nameof(Ordre), IsUnique = true)]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public class SessionEtude : AuditableAndSoftDeletableEntity, IEquatable<SessionEtude>, IComparable<SessionEtude>
{
    public SessionEtude() { }

    public SessionEtude(ushort annee, Saison saison) : this()
    {
        Annee = annee;
        Saison = saison;
        Ordre = (ushort)((Annee * 10) + saison.GetOrdre());
    }

    public ushort Annee { get; private set; } = default!;
    public Saison Saison { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public ushort Ordre { get; private set; } = default!;

    public int CompareTo(SessionEtude? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return ReferenceEquals(null, other) ? 1 : Ordre.CompareTo(other.Ordre);
    }

    public bool Equals(SessionEtude? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (Annee == other.Annee && Saison == other.Saison) || (Id != Guid.Empty && Id.Equals(other.Id));
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

        return obj.GetType() == GetType() && Equals((SessionEtude)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Annee, (int)Saison);
    }

    public static bool operator ==(SessionEtude? a, SessionEtude? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(SessionEtude? a, SessionEtude? b)
    {
        return !(a == b);
    }

    public void SetSlug(string value)
    {
        Slug = value.ToUpperInvariant();
    }
}