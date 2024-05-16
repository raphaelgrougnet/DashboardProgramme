using Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Programmes;

[Index(nameof(Numero), IsUnique = true)]
public class Programme : AuditableEntity, ISanitizable, IEquatable<Programme>
{
    public Programme() { }

    public Programme(string numero, string nom, Programme? alias = null) : this()
    {
        Numero = numero;
        Nom = nom;

        if (alias is not null)
        {
            SetEstAliasDe(alias);
        }
    }

    public string Nom { get; private set; } = default!;
    public string Numero { get; private set; } = default!;
    public Programme? EstAliasDe { get; private set; }

    public bool Equals(Programme? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Numero == other.Numero || (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void SanitizeForSaving()
    {
        Nom = Nom.Trim();
        Numero = Numero.ToUpper().Trim();
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

        return obj.GetType() == GetType() && Equals((Programme)obj);
    }

    public override int GetHashCode()
    {
        return Numero.GetHashCode();
    }

    public static bool operator ==(Programme? a, Programme? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(Programme? a, Programme? b)
    {
        return !(a == b);
    }

    public void SetEstAliasDe(Programme estAliasDe)
    {
        if (ReferenceEquals(this, estAliasDe))
        {
            throw new InvalidOperationException("Un programme ne peut pas être un alias de lui-même.");
        }

        EstAliasDe = estAliasDe;
    }

    public void SetNom(string nom)
    {
        Nom = nom;
    }

    public void SetNumero(string numero)
    {
        Numero = numero;
    }
}