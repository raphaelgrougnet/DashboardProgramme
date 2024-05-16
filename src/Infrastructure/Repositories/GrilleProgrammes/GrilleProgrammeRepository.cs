using Application.Exceptions.GrilleProgramme;
using Application.Interfaces;

using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Programmes;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.GrilleProgrammes;

public class GrilleProgrammeRepository(IDashboardProgrammeDbContext context) : IGrilleProgrammeRepository
{
    public async Task<GrilleProgramme> AddGrilleProgrammeWithoutSaving(GrilleProgramme pGrilleProgramme)
    {
        if (context.GrilleProgrammes.IgnoreQueryFilters()
                .FirstOrDefault(x =>
                    x.Programme.Id == pGrilleProgramme.Programme.Id &&
                    x.EtaleeSurNbSessions == pGrilleProgramme.EtaleeSurNbSessions &&
                    x.AnneeMiseAJour == pGrilleProgramme.AnneeMiseAJour) is not null)
        {
            throw new GrilleProgrammeAlreadyExistsException("A grilleProgramme already exists.");
        }


        context.GrilleProgrammes.Add(pGrilleProgramme);
        return pGrilleProgramme;
    }

    public async Task<GrilleProgramme?> FindByParams(Programme programme, byte etaleeSurNbSessions,
        ushort anneeMiseAJour)
    {
        IEnumerable<EntityEntry<GrilleProgramme>> pendingChanges = context.EfChangeTracker.Entries<GrilleProgramme>();
        GrilleProgramme? grillePENattenteDeSauvegarde =
            pendingChanges.FirstOrDefault(c => c.Entity.Programme.Id == programme.Id
                                               && c.Entity.EtaleeSurNbSessions == etaleeSurNbSessions
                                               && c.Entity.AnneeMiseAJour == anneeMiseAJour)?.Entity;

        if (grillePENattenteDeSauvegarde is not null)
        {
            return grillePENattenteDeSauvegarde;
        }

        GrilleProgramme? gp =
            await context.GrilleProgrammes
                .FirstOrDefaultAsync(x =>
                    x.Programme.Id == programme.Id && x.AnneeMiseAJour == anneeMiseAJour &&
                    x.EtaleeSurNbSessions == etaleeSurNbSessions);
        return gp;
    }


    public GrilleProgramme FindById(Guid id)
    {
        GrilleProgramme? grilleProgramme = context.GrilleProgrammes
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (grilleProgramme == null)
        {
            throw new GrilleProgrammeNotFoundException($"Could not find grilleProgramme with id {id}.");
        }

        return grilleProgramme;
    }

    public List<GrilleProgramme> GetAll()
    {
        return context.GrilleProgrammes.AsNoTracking().ToList();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}