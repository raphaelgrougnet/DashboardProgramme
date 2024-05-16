using Domain.Entities;
using Domain.Entities.Books;
using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.CoursSecondaireReussis;
using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Members;
using Domain.Entities.Programmes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Interfaces;

public interface IDashboardProgrammeDbContext
{
    DbSet<Member> Members { get; }
    DbSet<Book> Books { get; }
    DbSet<Programme> Programmes { get; }
    DbSet<Etudiant> Etudiants { get; }
    DbSet<CoursSecondaireReussi> CoursSecondaireReussis { get; }

    DbSet<Cours> Cours { get; set; }
    DbSet<CoursAssiste> CoursAssistes { get; set; }
    DbSet<SessionEtude> SessionEtudes { get; }
    DbSet<SessionAssistee> SessionAssistees { get; }

    DbSet<GrilleProgramme> GrilleProgrammes { get; }
    DbSet<MemberProgramme> MemberProgrammes { get; }
    DbSet<GrilleProgrammeCours> GrilleProgrammeCours { get; }

    ChangeTracker EfChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken? cancellationToken = null);

    void SetRequestTimeout(ushort? timeout = null);
}