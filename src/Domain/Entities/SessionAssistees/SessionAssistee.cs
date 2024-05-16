using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.CoursAssistes;
using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.SessionEtudes;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.SessionAssistees;

[Index(nameof(Etudiant) + "Id", nameof(SessionEtude) + "Id", IsUnique = true)]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public class SessionAssistee : AuditableEntity, IEquatable<SessionAssistee>, IComparable<SessionAssistee>
{
    public SessionAssistee()
    {
        CoursAssistes = [];
    }

    public SessionAssistee(Etudiant etudiant, GrilleProgramme grilleProgramme, SessionEtude sessionEtude,
        ushort nbTotalHeures, byte niemeSession, bool estBeneficiaireServicesAdaptes) : this()
    {
        Etudiant = etudiant;
        GrilleProgramme = grilleProgramme;
        SessionEtude = sessionEtude;
        NbTotalHeures = nbTotalHeures;
        NiemeSession = niemeSession;
        EstBeneficiaireServicesAdaptes = estBeneficiaireServicesAdaptes;
    }


    public Etudiant Etudiant { get; private set; } = default!;
    public GrilleProgramme GrilleProgramme { get; private set; } = default!;
    public HashSet<CoursAssiste> CoursAssistes { get; private set; } = default!;
    public SessionEtude SessionEtude { get; private set; } = default!;
    public ushort NbTotalHeures { get; private set; } = default!;
    public byte NiemeSession { get; private set; } = default!;
    public bool EstBeneficiaireServicesAdaptes { get; private set; } = default!;


    public int CompareTo(SessionAssistee? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return ReferenceEquals(null, other) ? 1 : NiemeSession.CompareTo(other.NiemeSession);
    }

    public bool Equals(SessionAssistee? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (Etudiant.Equals(other.Etudiant) && SessionEtude.Equals(other.SessionEtude)) ||
               (Id != Guid.Empty && Id.Equals(other.Id));
    }

    public void AddCoursAssiste(CoursAssiste coursAssiste)
    {
        coursAssiste.SetSessionAssistee(this);
        CoursAssistes.Add(coursAssiste);
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

        return obj.GetType() == GetType() && Equals((SessionAssistee)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Etudiant, SessionEtude);
    }

    public static bool operator ==(SessionAssistee? a, SessionAssistee? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(SessionAssistee? a, SessionAssistee? b)
    {
        return !(a == b);
    }

    public void SetEtudiant(Etudiant etudiant)
    {
        Etudiant = etudiant;
    }
}