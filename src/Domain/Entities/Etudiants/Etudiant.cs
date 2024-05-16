using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.CoursSecondaireReussis;
using Domain.Entities.SessionAssistees;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Etudiants;

[Index(nameof(HashCodePermanent), IsUnique = true)]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
public class Etudiant : AuditableEntity, ISanitizable, IEquatable<Etudiant>
{
    public Etudiant()
    {
        CoursSecondaireReussis = [];
        SessionAssistees = [];
    }

    public Etudiant(string hashCodePermanent, bool estBeneficiaireRenforcementFr, byte tourAdmission,
        StatutImmigration statutImmigration, Population population, Sanction sanction,
        float moyenneGeneraleAuSecondaire, bool estAssujetiAuR18) : this()
    {
        HashCodePermanent = hashCodePermanent;
        EstBeneficiaireRenforcementFr = estBeneficiaireRenforcementFr;
        TourAdmission = tourAdmission;
        StatutImmigration = statutImmigration;
        Population = population;
        Sanction = sanction;
        MoyenneGeneraleAuSecondaire = moyenneGeneraleAuSecondaire;
        EstAssujetiAuR18 = estAssujetiAuR18;
    }

    public string HashCodePermanent { get; private set; } = default!;
    public bool EstBeneficiaireRenforcementFr { get; private set; } = default!;
    public byte TourAdmission { get; private set; } = default!;
    public StatutImmigration StatutImmigration { get; private set; } = default!;
    public Population Population { get; private set; } = default!;
    public Sanction Sanction { get; private set; } = default!;
    public float MoyenneGeneraleAuSecondaire { get; private set; } = default!;
    public bool EstAssujetiAuR18 { get; private set; } = default!;
    public HashSet<CoursSecondaireReussi> CoursSecondaireReussis { get; private set; } = default!;
    public SortedSet<SessionAssistee> SessionAssistees { get; private set; } = default!;

    public bool Equals(Etudiant? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return HashCodePermanent == other.HashCodePermanent || (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void SanitizeForSaving()
    {
        HashCodePermanent = HashCodePermanent.ToUpper().Trim();
    }

    public void AddCoursSecondaireReussi(CoursSecondaireReussi coursSecondaireReussi)
    {
        coursSecondaireReussi.SetEtudiant(this);
        CoursSecondaireReussis.Add(coursSecondaireReussi);
    }

    public void AddSessionAssistee(SessionAssistee sessionAssistee)
    {
        sessionAssistee.SetEtudiant(this);
        SessionAssistees.Add(sessionAssistee);
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

        return obj.GetType() == GetType() && Equals((Etudiant)obj);
    }

    public override int GetHashCode()
    {
        return HashCodePermanent.GetHashCode();
    }

    public static bool operator ==(Etudiant? a, Etudiant? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(Etudiant? a, Etudiant? b)
    {
        return !(a == b);
    }

    public void SetEstAssujetiAuR18(bool estAssujetiAuR18)
    {
        EstAssujetiAuR18 = estAssujetiAuR18;
    }

    public void SetPopulation(Population population)
    {
        Population = population;
    }

    public void SetSanction(Sanction sanction)
    {
        Sanction = sanction;
    }

    public void SetStatutImmigration(StatutImmigration statutImmigration)
    {
        StatutImmigration = statutImmigration;
    }
}