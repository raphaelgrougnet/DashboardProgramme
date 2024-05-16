using Domain.Common;
using Domain.Entities.Etudiants;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.CoursSecondaireReussis;

[Index(nameof(CodeCours))]
[Index(nameof(CodeCours), nameof(Etudiant) + "Id", IsUnique = true)]
public class CoursSecondaireReussi : AuditableEntity, ISanitizable, IEquatable<CoursSecondaireReussi>
{
    public CoursSecondaireReussi()
    {
    }

    public CoursSecondaireReussi(Etudiant etudiant, string codeCours) : this()
    {
        Etudiant = etudiant;
        CodeCours = codeCours;
    }


    public Etudiant Etudiant { get; private set; } = default!;
    public string CodeCours { get; private set; } = default!;


    public bool Equals(CoursSecondaireReussi? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (Etudiant.Equals(other.Etudiant) && CodeCours == other.CodeCours) ||
               (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void SanitizeForSaving()
    {
        CodeCours = CodeCours.ToUpper().Trim();
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

        return obj.GetType() == GetType() && Equals((CoursSecondaireReussi)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Etudiant, CodeCours);
    }

    public static bool operator ==(CoursSecondaireReussi? a, CoursSecondaireReussi? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(CoursSecondaireReussi? a, CoursSecondaireReussi? b)
    {
        return !(a == b);
    }

    public void SetEtudiant(Etudiant etudiant)
    {
        Etudiant = etudiant;
    }
}