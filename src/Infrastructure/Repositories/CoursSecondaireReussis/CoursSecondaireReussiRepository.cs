using Application.Exceptions.CoursSecondaireReussis;
using Application.Interfaces;

using Domain.Entities.CoursSecondaireReussis;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.CoursSecondaireReussis;

public class CoursSecondaireReussiRepository(IDashboardProgrammeDbContext context) : ICoursSecondaireReussiRepository
{
    public async Task CreateCoursSecondaireReussi(CoursSecondaireReussi cours)
    {
        await CreateCoursSecondaireReussiteWithoutSaveChanges(cours);
    }

    public async Task CreateCoursSecondaireReussiteWithoutSaveChanges(CoursSecondaireReussi cours)
    {
        CoursSecondaireReussi? existingCours = FindByEtudiantAndCodeCourse(cours.Etudiant.Id, cours.CodeCours);
        if (existingCours is not null)
        {
            throw new CoursSecondaireReussiAlreadyExistsException(
                $"Cours du secondaire réussi with code {cours.CodeCours} already exists for etudiant {cours.Etudiant.Id}.");
        }

        context.CoursSecondaireReussis.Add(cours);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteCoursSecondaireReussiWithId(Guid id)
    {
        CoursSecondaireReussi? cours = context.CoursSecondaireReussis.FirstOrDefault(x => x.Id == id);
        if (cours == null)
        {
            throw new CoursSecondaireReussiNotFoundException(
                $"Could not find cours du secondaire réussi with id {id}.");
        }

        context.CoursSecondaireReussis.Remove(cours);
        await context.SaveChangesAsync();
    }

    public CoursSecondaireReussi? FindByEtudiantAndCodeCourse(Guid etudiantId, string codeCours)
    {
        EntityEntry<CoursSecondaireReussi>? coursEnAttenteDeSauvegarde =
            context.EfChangeTracker.Entries<CoursSecondaireReussi>().FirstOrDefault(c => c.Entity.CodeCours == codeCours
                && c.Entity.Etudiant.Id == etudiantId);
        if (coursEnAttenteDeSauvegarde is not null)
        {
            return coursEnAttenteDeSauvegarde.Entity;
        }


        return context.CoursSecondaireReussis
            .AsNoTracking()
            .FirstOrDefault(x => x.Etudiant.Id == etudiantId && x.CodeCours == codeCours);
    }

    public CoursSecondaireReussi FindById(Guid id)
    {
        CoursSecondaireReussi? cours = context.CoursSecondaireReussis
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (cours == null)
        {
            throw new CoursSecondaireReussiNotFoundException(
                $"Could not find cours du secondaire réussi with id {id}.");
        }

        return cours;
    }

    public List<CoursSecondaireReussi> GetAll()
    {
        return context.CoursSecondaireReussis.AsNoTracking().ToList();
    }
}