using Application.Exceptions.Etudiants;
using Application.Interfaces;

using Domain.Entities.Etudiants;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.Etudiants;

public class EtudiantRepository(IDashboardProgrammeDbContext context) : IEtudiantRepository
{
    public async Task CreateEtudiant(Etudiant etudiant)
    {
        if (context.Etudiants.Any(x => x.HashCodePermanent.Trim() == etudiant.HashCodePermanent.Trim()))
        {
            throw new EtudiantWithCodePermanentAlreadyExistsException(
                $"An etudiant with code permanent {etudiant.HashCodePermanent} already exists.");
        }

        context.Etudiants.Add(etudiant);
        await context.SaveChangesAsync();
    }

    public Etudiant FindById(Guid id)
    {
        EntityEntry<Etudiant>? etudiantEnAttenteDeSauvegarde =
            context.EfChangeTracker.Entries<Etudiant>().FirstOrDefault(c => c.Entity.Id == id);
        if (etudiantEnAttenteDeSauvegarde is not null)
        {
            return etudiantEnAttenteDeSauvegarde.Entity;
        }

        Etudiant? etudiant = context.Etudiants
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (etudiant == null)
        {
            throw new EtudiantNotFoundException($"Could not find etudiant with id {id}.");
        }

        return etudiant;
    }

    public List<Etudiant> GetAll()
    {
        return context.Etudiants.AsNoTracking().ToList();
    }

    public async Task UpdateEtudiant(Etudiant etudiant)
    {
        if (context.Etudiants.Any(x =>
                x.HashCodePermanent == etudiant.HashCodePermanent.Trim() && x.Id != etudiant.Id))
        {
            throw new EtudiantWithCodePermanentAlreadyExistsException(
                $"Another etudiant with code permanent {etudiant.HashCodePermanent} already exists.");
        }

        context.Etudiants.Update(etudiant);
        await context.SaveChangesAsync();
    }

    public Etudiant? FindByHashCodePermanent(string hashCodePermanent)
    {
        EntityEntry<Etudiant>? etudiantEnAttenteDeSauvegarde =
            context.EfChangeTracker.Entries<Etudiant>()
                .FirstOrDefault(c => c.Entity.HashCodePermanent == hashCodePermanent);

        if (etudiantEnAttenteDeSauvegarde is not null)
        {
            return etudiantEnAttenteDeSauvegarde.Entity;
        }

        return context.Etudiants
            .FirstOrDefault(x => x.HashCodePermanent == hashCodePermanent);
    }

    public async Task<Etudiant> AddEtudiantWIthoutSaveChanges(Etudiant etudiant)
    {
        if (context.Etudiants.Any(x => x.HashCodePermanent.Trim() == etudiant.HashCodePermanent.Trim()))
        {
            throw new EtudiantWithCodePermanentAlreadyExistsException(
                $"An etudiant with code permanent {etudiant.HashCodePermanent} already exists.");
        }

        context.Etudiants.Add(etudiant);
        return etudiant;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}