using System.Diagnostics.CodeAnalysis;

using Domain.Common;
using Domain.Entities.CoursNs;
using Domain.Entities.SessionAssistees;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.CoursAssistes;

[Index(nameof(SessionAssistee) + "Id", nameof(Cours) + "Id", IsUnique = true)]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
public class CoursAssiste : AuditableEntity, IEquatable<CoursAssiste>
{
    public CoursAssiste()
    {
    }

    public CoursAssiste(Cours cours, Note note, byte groupe, SessionAssistee sessionAssistee) : this()
    {
        Cours = cours;
        NoteRecue = note;
        NumeroGroupe = groupe;
        SessionAssistee = sessionAssistee;
    }

    public CoursAssiste(Cours cours, Note note, SessionAssistee sessionAssistee) : this()
    {
        Cours = cours;
        NoteRecue = note;
        SessionAssistee = sessionAssistee;
    }


    public Cours Cours { get; private set; } = default!;
    public Note NoteRecue { get; private set; }
    public byte NumeroGroupe { get; private set; }
    public SessionAssistee SessionAssistee { get; private set; } = default!;

    public bool Equals(CoursAssiste? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (Cours.Equals(other.Cours) && SessionAssistee.Equals(other.SessionAssistee)) ||
               (Id != Guid.Empty && Id.Equals(other.Id));
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

        return obj.GetType() == GetType() && Equals((CoursAssiste)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Cours, SessionAssistee);
    }

    public static bool operator ==(CoursAssiste? a, CoursAssiste? b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(CoursAssiste? a, CoursAssiste? b)
    {
        return !(a == b);
    }

    public void SetSessionAssistee(SessionAssistee sessionAssistee)
    {
        SessionAssistee = sessionAssistee;
    }
}