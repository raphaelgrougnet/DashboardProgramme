using Application.Exceptions.CoursAssistes;
using Application.Exceptions.SessionEtudes;
using Application.Interfaces;

using Domain.Entities.CoursAssistes;
using Domain.Entities.Etudiants;
using Domain.Entities.SessionEtudes;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.CoursAssistes;

public class CoursAssisteRepository(IDashboardProgrammeDbContext context, ILogger<CoursAssisteRepository> logger)
    : ICoursAssisteRepository
{
    private readonly ILogger<CoursAssisteRepository> _logger = logger;

    public async Task CreateCoursAssiste(CoursAssiste coursAssiste)
    {
        await CreateCoursAssisteWithoutSaveChanges(coursAssiste);
    }

    public async Task<CoursAssiste> CreateCoursAssisteWithoutSaveChanges(CoursAssiste coursAssiste)
    {
        if (context.CoursAssistes.Any(x =>
                x.SessionAssistee.Id == coursAssiste.SessionAssistee.Id && x.Cours.Id == coursAssiste.Cours.Id))
        {
            throw new CoursAssisteAlreadyExistsException(
                "A cours assisté with meme session assistée, cours et groupe already exists.");
        }


        context.CoursAssistes.Add(coursAssiste);
        return coursAssiste;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public CoursAssiste FindById(Guid id)
    {
        CoursAssiste? cours = context.CoursAssistes
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (cours is null)
        {
            throw new CoursAssisteNotFoundException($"Could not find cours with id {id}.");
        }

        return cours;
    }

    public List<CoursAssiste> FindBySessionAssisteeId(Guid id)
    {
        List<CoursAssiste> cours = context.CoursAssistes
            .AsNoTracking()
            .Where(x => x.SessionAssistee.Id == id).ToList();
        if (cours == null)
        {
            throw new CoursAssisteNotFoundException($"Could not find cours with sessionAssiste Id {id}.");
        }

        return cours;
    }

    public List<CoursAssiste> GetAll()
    {
        return context.CoursAssistes.AsNoTracking().ToList();
    }

    public Dictionary<Guid, Dictionary<Note, int>> GetReussiteParCoursPourProgrammeEtSession(Guid idProgramme,
        Guid idSessionEtude)
    {
        return context.CoursAssistes
            .Where(ca =>
                ca.SessionAssistee.SessionEtude.Id == idSessionEtude &&
                ca.SessionAssistee.GrilleProgramme.Programme.Id == idProgramme)
            .GroupBy(ca => new { ca.Cours.Id, ca.NoteRecue })
            .Select(g => new { CoursId = g.Key.Id, g.Key.NoteRecue, QteAvecCetteNote = g.Count() })
            .ToLookup(g => g.CoursId)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(k => k.NoteRecue, k => k.QteAvecCetteNote)
            );
    }

    public CoursAssiste? FindBySessionAssisteeIdAndCoursId(Guid idSessionAssistee, Guid idCours)
    {
        EntityEntry<CoursAssiste>? coursEnAttenteDeSauvegarde =
            context.EfChangeTracker.Entries<CoursAssiste>().FirstOrDefault(c =>
                c.Entity.SessionAssistee.Id == idSessionAssistee && c.Entity.Cours.Id == idCours);
        if (coursEnAttenteDeSauvegarde is not null)
        {
            return coursEnAttenteDeSauvegarde.Entity;
        }

        return context.CoursAssistes
            .FirstOrDefault(x => x.SessionAssistee.Id == idSessionAssistee && x.Cours.Id == idCours);
    }

    public Dictionary<Guid, Dictionary<Note, int>> GetReussiteParSessionPourProgramme(Guid idProgramme)
    {
        ushort anneeMin = context.SessionEtudes.OrderByDescending(s => s.Annee).Take(6).Min(s => s.Annee);

        return context.CoursAssistes
            .Where(ca =>
                ca.SessionAssistee.GrilleProgramme.Programme.Id == idProgramme &&
                ca.SessionAssistee.SessionEtude.Annee >= anneeMin)
            .GroupBy(ca => new { ca.SessionAssistee.SessionEtude.Id, ca.NoteRecue })
            .Select(g => new { g.Key.Id, g.Key.NoteRecue, QteAvecCetteNote = g.Count() })
            .OrderByDescending(result => result.Id)
            .ThenByDescending(result => result.NoteRecue)
            .ToLookup(g => g.Id)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(k => k.NoteRecue, k => k.QteAvecCetteNote)
            );

        /*
         SELECT [SessionEtudeId], [NoteRecue], Count([NoteRecue]) AS [QteAvecCetteNote] FROM [dbo].[CoursAssistes] AS [ca]
           INNER JOIN (
               SELECT [sa].[Id] AS [SessionAssisteeId], [SessionEtudeId] FROM [dbo].[SessionAssistees] AS [sa]
               INNER JOIN [dbo].[GrilleProgrammes] AS [gp]
               ON [sa].[GrilleProgrammeId] = [gp].Id
               WHERE [ProgrammeId] = 'a28267dd-4253-46f6-a95f-08dc323ff1e0'
           ) AS [jo]
               ON [ca].[SessionAssisteeId] = [jo].[SessionAssisteeId]
           GROUP BY [SessionEtudeId], [NoteRecue]
           ORDER BY [SessionEtudeId], [NoteRecue] DESC;

           Donne les colonnes SessionEtudeId, NoteRecue, QteAvecCetteNote
         */
    }

    public Dictionary<Note, int> GetReussitePourCoursDeSessionDeProgramme(Guid idProgramme,
        Guid idSession, Guid idCours)
    {
        Dictionary<Note, int> query = context.CoursAssistes
            .Where(ca => ca.SessionAssistee.SessionEtude.Id == idSession &&
                         ca.SessionAssistee.GrilleProgramme.Programme.Id == idProgramme &&
                         ca.Cours.Id == idCours)
            .GroupBy(ca => ca.NoteRecue)
            .Select(n => new { n.Key, QteAvecCetteNote = n.Count() })
            .ToDictionary(k => k.Key, v => v.QteAvecCetteNote);

        return query;
    }

    public Dictionary<Note, int> GetReussitePourCoursEntre2Sessions(Guid idProgramme, Guid
        idCours, Guid sessionDebutId, Guid sessionFinId)
    {
        SessionEtude? sessionDebut = context.SessionEtudes.FirstOrDefault(se => se.Id == sessionDebutId);
        SessionEtude? sessionFin = context.SessionEtudes.FirstOrDefault(se => se.Id == sessionFinId);

        if (sessionDebut is null || sessionFin is null)
        {
            throw new SessionEtudeNotFoundException("Could not find session etude for comparison.");
        }

        Dictionary<Note, int> query = context.CoursAssistes
            .Where(ca => ca.SessionAssistee.GrilleProgramme.Programme.Id == idProgramme &&
                         ca.Cours.Id == idCours &&
                         ca.SessionAssistee.SessionEtude.Saison != Saison.Ete &&
                         ca.SessionAssistee.SessionEtude.Ordre >= sessionDebut.Ordre &&
                         ca.SessionAssistee.SessionEtude.Ordre <= sessionFin.Ordre)
            .GroupBy(ca => ca.NoteRecue)
            .Select(n => new { n.Key, QteAvecCetteNote = n.Count() })
            .ToDictionary(k => k.Key, v => v.QteAvecCetteNote);

        return query;
    }
}