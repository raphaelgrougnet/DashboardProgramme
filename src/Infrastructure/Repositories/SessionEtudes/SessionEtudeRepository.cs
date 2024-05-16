using Application.Exceptions.SessionEtudes;
using Application.Interfaces;

using Domain.Entities.SessionEtudes;
using Domain.Extensions;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;

using Slugify;

namespace Infrastructure.Repositories.SessionEtudes;

public class SessionEtudeRepository(IDashboardProgrammeDbContext context, ISlugHelper slugHelper)
    : ISessionEtudeRepository
{
    public async Task<SessionEtude> CreateSessionEtude(SessionEtude sessionEtude)
    {
        return await AddSessionEtudeWithoutSaving(sessionEtude);
    }

    public SessionEtude? FindByAnneeAndSaison(ushort annee, Saison saison)
    {
        SessionEtude? sessionEtEnAttenteSauvegarde = context.EfChangeTracker.Entries<SessionEtude>()
            .FirstOrDefault(c => c.Entity.Annee == annee && c.Entity.Saison == saison)?.Entity;

        if (sessionEtEnAttenteSauvegarde is not null)
        {
            return sessionEtEnAttenteSauvegarde;
        }

        return context.SessionEtudes.FirstOrDefault(x => x.Annee == annee && x.Saison == saison);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<SessionEtude> AddSessionEtudeWithoutSaving(SessionEtude sessionEtude)
    {
        if (context.SessionEtudes.Any(x => x.Saison == sessionEtude.Saison && x.Annee == sessionEtude.Annee))
        {
            throw new SessionEtudeWithSaisonAndAnneeAlreadyExistsException(
                $"A sessionEtude with saison and annee {sessionEtude.Saison}{sessionEtude.Annee} already exists.");
        }

        GenerateSlug(sessionEtude);
        context.SessionEtudes.Add(sessionEtude);

        return sessionEtude;
    }

    public SessionEtude FindById(Guid id)
    {
        SessionEtude? sessionEtude = context.SessionEtudes
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (sessionEtude is null)
        {
            throw new SessionEtudeNotFoundException($"Could not find sessionEtude with id {id}.");
        }

        return sessionEtude;
    }

    public List<SessionEtude> GetAll()
    {
        return context.SessionEtudes.AsNoTracking().ToList();
    }

    public IList<SessionEtude> GetAllForProgrammeId(Guid idProgramme)
    {
        return context.SessionEtudes.AsNoTracking().Where(se =>
                context.SessionAssistees.AsNoTracking()
                    .Any(sa => sa.SessionEtude == se && sa.GrilleProgramme.Programme.Id == idProgramme))
            .OrderByDescending(se => se.Annee).ToList();
    }

    public async Task<SessionEtude> GetLatestForProgrammeId(Guid idProgramme)
    {
        return await context.SessionEtudes.AsNoTracking()
            .Where(se => context.SessionAssistees.AsNoTracking().Any(sa =>
                sa.SessionEtude == se && sa.GrilleProgramme.Programme.Id == idProgramme))
            .OrderByDescending(se => se.Annee).FirstAsync();
    }

    private void GenerateSlug(SessionEtude sessionEtude)
    {
        int annee = sessionEtude.Annee is < 2100 and > 2000 ? sessionEtude.Annee % 2000 : sessionEtude.Annee;
        string slug = $"{sessionEtude.Saison.ToString().ToUpper()[0]}{annee}";
        List<SessionEtude> slugs = context.SessionEtudes.AsNoTracking().Where(x => x.Slug == slug).ToList();
        if (slugs.Count != 0)
        {
            slug = $"{sessionEtude.Saison.ToString().CapitalizeFirstLetterOfEachWord()}{sessionEtude.Annee}";
        }

        sessionEtude.SetSlug(slugHelper.GenerateSlug(slug).Replace(".", ""));
    }
}