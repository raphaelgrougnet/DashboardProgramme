using Domain.Common;

using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Domain.Entities.CoursNs;

[Index(nameof(Code), IsUnique = true)]
public class Cours : AuditableEntity, ISanitizable, IEquatable<Cours>
{
    public Cours() { }

    public Cours(string code, string nom) : this()
    {
        Nom = nom;
        Code = code;
    }

    public string Nom { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    public bool Equals(Cours? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Code == other.Code || (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void SanitizeForSaving()
    {
        Code = Code.ToUpper().Trim();
        Nom = Nom.Trim();
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

        return obj.GetType() == GetType() && Equals((Cours)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code);
    }

    public static bool operator ==(Cours? a, Cours? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(Cours? a, Cours? b)
    {
        return !(a == b);
    }
}