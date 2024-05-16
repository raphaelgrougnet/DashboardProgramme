using System.Reflection;

using Application.Interfaces;

using Domain.Common;
using Domain.Entities;
using Domain.Entities.Books;
using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.CoursSecondaireReussis;
using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Identity;
using Domain.Entities.Members;
using Domain.Entities.Programmes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

using Persistence.Extensions;
using Persistence.Interceptors;

namespace Persistence;

public class DashboardProgrammeDbContext : IdentityDbContext<User, Role, Guid,
    IdentityUserClaim<Guid>, UserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IDashboardProgrammeDbContext
{
    private readonly AuditableAndSoftDeletableEntitySaveChangesInterceptor
        _auditableAndSoftDeletableEntitySaveChangesInterceptor = default!;

    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor = default!;
    private readonly EntitySaveChangesInterceptor _entitySaveChangesInterceptor = default!;
    private readonly UserSaveChangesInterceptor _userSaveChangesInterceptor = default!;

    public DashboardProgrammeDbContext(
        DbContextOptions<DashboardProgrammeDbContext> options,
        AuditableAndSoftDeletableEntitySaveChangesInterceptor auditableAndSoftDeletableEntitySaveChangesInterceptor,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        UserSaveChangesInterceptor userSaveChangesInterceptor,
        EntitySaveChangesInterceptor entitySaveChangesInterceptor)
        : base(options)
    {
        _auditableAndSoftDeletableEntitySaveChangesInterceptor = auditableAndSoftDeletableEntitySaveChangesInterceptor;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _userSaveChangesInterceptor = userSaveChangesInterceptor;
        _entitySaveChangesInterceptor = entitySaveChangesInterceptor;
    }

    public DashboardProgrammeDbContext()
    {
    }

    public DashboardProgrammeDbContext(DbContextOptions<DashboardProgrammeDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; } = default!;
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Programme> Programmes { get; set; } = default!;
    public DbSet<SessionEtude> SessionEtudes { get; set; } = default!;
    public DbSet<SessionAssistee> SessionAssistees { get; set; } = default!;

    public DbSet<Cours> Cours { get; set; } = default!;
    public DbSet<CoursAssiste> CoursAssistes { get; set; } = default!;
    public DbSet<Etudiant> Etudiants { get; set; } = default!;
    public DbSet<CoursSecondaireReussi> CoursSecondaireReussis { get; set; } = default!;
    public DbSet<GrilleProgramme> GrilleProgrammes { get; set; } = default!;
    public DbSet<MemberProgramme> MemberProgrammes { get; set; } = default!;
    public DbSet<GrilleProgrammeCours> GrilleProgrammeCours { get; set; } = default!;

    public void SetRequestTimeout(ushort? timeout = null)
    {
        Database.SetCommandTimeout(timeout);
    }

    public async Task<int> SaveChangesAsync(CancellationToken? cancellationToken = null)
    {
        return await base.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
    }

    public ChangeTracker EfChangeTracker => ChangeTracker;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            _auditableAndSoftDeletableEntitySaveChangesInterceptor,
            _auditableEntitySaveChangesInterceptor,
            _userSaveChangesInterceptor,
            _entitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Etudiant>().Property(e => e.Population).HasConversion<string>();
        builder.Entity<Etudiant>().Property(e => e.Sanction).HasConversion<string>();
        builder.Entity<Etudiant>().Property(e => e.StatutImmigration).HasConversion<string>();

        builder.Entity<SessionEtude>().Property(s => s.Saison).HasConversion<string>();

        builder.Entity<CoursAssiste>().Property(c => c.NoteRecue).HasConversion<string>();

        base.OnModelCreating(builder);

        // Global query to prevent loading soft-deleted entities
        foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            if (entityType.ClrType == typeof(User))
            {
                continue;
            }

            entityType.AddSoftDeleteQueryFilter();
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}