using Application.Exceptions.SessionAssistees;
using Application.Interfaces;

using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.SessionAssistees;

public class SessionAssisteeRepository(IDashboardProgrammeDbContext context) : ISessionAssisteeRepository
{
    public async Task<SessionAssistee> AddSessionAssisteeWithoutSaving(SessionAssistee sessionAssistee)
    {
        SessionAssistee? existingSessionAssistee = context.SessionAssistees
            .AsNoTracking()
            .FirstOrDefault(x =>
                x.Etudiant.Id == sessionAssistee.Etudiant.Id && x.SessionEtude.Id == sessionAssistee.Etudiant.Id
                                                             && x.NbTotalHeures == sessionAssistee.NbTotalHeures &&
                                                             x.NiemeSession == sessionAssistee.NiemeSession
                                                             && x.EstBeneficiaireServicesAdaptes ==
                                                             sessionAssistee.EstBeneficiaireServicesAdaptes &&
                                                             x.GrilleProgramme.Id ==
                                                             sessionAssistee.GrilleProgramme.Id);

        if (existingSessionAssistee is not null)
        {
            throw new SessionAssisteeExistException("Session assistee already exists.");
        }


        context.SessionAssistees.Add(sessionAssistee);
        return sessionAssistee;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public SessionAssistee? FindByParams(Etudiant etudiant, GrilleProgramme grilleProgramme, SessionEtude se,
        ushort nbHeures, byte nIemeSession,
        bool estBeneficiaireServicesAdaptes)
    {
        SessionAssistee? sessionAEnAttenteAjout = context.EfChangeTracker.Entries<SessionAssistee>()
            .FirstOrDefault(c => c.Entity.Etudiant.Id == etudiant.Id
                                 && c.Entity.GrilleProgramme.Id == grilleProgramme.Id
                                 && c.Entity.SessionEtude.Id == se.Id && c.Entity.NbTotalHeures == nbHeures &&
                                 c.Entity.NiemeSession == nIemeSession
                                 && c.Entity.EstBeneficiaireServicesAdaptes == estBeneficiaireServicesAdaptes)?.Entity;

        if (sessionAEnAttenteAjout is not null)
        {
            return sessionAEnAttenteAjout;
        }

        return context.SessionAssistees
            .FirstOrDefault(x => x.Etudiant.Id == etudiant.Id && x.SessionEtude.Id == se.Id &&
                                 x.NbTotalHeures == nbHeures &&
                                 x.NiemeSession == nIemeSession &&
                                 x.EstBeneficiaireServicesAdaptes == estBeneficiaireServicesAdaptes);
    }

    public SessionAssistee FindById(Guid id)
    {
        SessionAssistee? sessionAssistee = context.SessionAssistees
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (sessionAssistee == null)
        {
            throw new SessionAssisteeNotFoundException($"Could not find sessionAssitee with id {id}.");
        }

        return sessionAssistee;
    }

    public List<SessionAssistee> GetAll()
    {
        return context.SessionAssistees.AsNoTracking().ToList();
    }

    public async Task<IDictionary<byte, double>> ObtenirMoyenneNbEtudiantsParSpePourProgramme(Guid programmeId)
    {
        return await context.SessionAssistees
            .AsNoTracking()
            .Where(sa => sa.GrilleProgramme.Programme.Id == programmeId)
            .GroupBy(sa => new { Spe = sa.NiemeSession, Session = sa.SessionEtude.Id })
            .Select(g => new { g.Key.Spe, NbAvecCeSpe = g.Count() })
            .GroupBy(g => g.Spe)
            .Select(g => new { Spe = g.Key, MoyenneNbAvecCeSpe = g.Average(x => x.NbAvecCeSpe) })
            .ToDictionaryAsync(g => g.Spe, g => g.MoyenneNbAvecCeSpe);
    }

    public async Task<IEnumerable<IDictionary<byte, int>>> ObtenirNbEtudiantsParSpeDesTroisDernieresSessions(Guid
        sessionEtudeId, Guid programmeId)
    {
        SessionEtude? sessionActuelle = await context.SessionEtudes
            .AsNoTracking()
            .FirstOrDefaultAsync(se => se.Id == sessionEtudeId);

        if (sessionActuelle is null)
        {
            return [];
        }

        List<Guid> sessionsPrecedentes = await context.SessionEtudes
            .AsNoTracking()
            .Where(se => se.Ordre < sessionActuelle.Ordre)
            .OrderByDescending(se => se.Ordre)
            .Take(3)
            .Select(se => se.Id)
            .ToListAsync();

        return sessionsPrecedentes.Select(async se =>
            await ObtenirNbEtudiantsParSpePourSessionEtudeDeProgramme(se, programmeId)).Select(t => t.Result);
    }

    public async Task<IDictionary<byte, int>> ObtenirNbEtudiantsParSpePourSessionEtudeDeProgramme(Guid sessionEtudeId,
        Guid
            programmeId)
    {
        return (await context.SessionAssistees
                .AsNoTracking()
                .Where(sa => sa.SessionEtude.Id == sessionEtudeId && sa.GrilleProgramme.Programme.Id == programmeId)
                .ToListAsync())
            .GroupBy(sa => sa.NiemeSession)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<int[]> ObtenirNbServicesAdaptesDesDixDernieresSessionsParProgramme(Guid programmeId)
    {
        return (await context.SessionAssistees
                .AsNoTracking()
                .Include(sa => sa.SessionEtude)
                .Where(sa => sa.GrilleProgramme.Programme.Id == programmeId
                             && sa.SessionEtude.Saison != Saison.Ete)
                .OrderByDescending(sa => sa.SessionEtude.Ordre)
                .ToListAsync())
            .GroupBy(s => s.SessionEtude.Id)
            .Take(10)
            .Reverse()
            .Select(g =>
                (int)((double)g.Count(s => s.EstBeneficiaireServicesAdaptes) / g.Count() * 100))
            .ToArray();
    }

    public async Task<int[]> ObtenirNbResidentsTemporairesDesDixDernieresSessionsParProgramme(Guid programmeId)
    {
        return (await context.SessionAssistees
                .AsNoTracking()
                .Include(sa => sa.SessionEtude)
                .Include(sa => sa.Etudiant)
                .Where(sa => sa.GrilleProgramme.Programme.Id == programmeId
                             && sa.SessionEtude.Saison != Saison.Ete)
                .OrderByDescending(sa => sa.SessionEtude.Ordre)
                .ToListAsync())
            .GroupBy(s => s.SessionEtude.Id)
            .Take(10)
            .Reverse()
            .Select(g =>
                (int)((double)g.Count(s => s.Etudiant.StatutImmigration == StatutImmigration.ResidentTemporaire) /
                    g.Count() * 100))
            .ToArray();
    }

    public async Task<int[]> ObtenirNbAssujetisR18DesDixDernieresSessionsParProgramme(Guid programmeId)
    {
        return (await context.SessionAssistees
                .AsNoTracking()
                .Include(sa => sa.SessionEtude)
                .Include(sa => sa.Etudiant)
                .Where(sa => sa.GrilleProgramme.Programme.Id == programmeId
                             && sa.SessionEtude.Saison != Saison.Ete)
                .OrderByDescending(sa => sa.SessionEtude.Ordre)
                .ToListAsync())
            .GroupBy(s => s.SessionEtude.Id)
            .Take(10)
            .Reverse()
            .Select(g =>
                (int)((double)g.Count(s => s.Etudiant.EstAssujetiAuR18) / g.Count() * 100))
            .ToArray();
    }
}