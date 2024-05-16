using Domain.Entities.Etudiants;

namespace Domain.Repositories;

public interface IEtudiantRepository
{
    List<Etudiant> GetAll();
    Etudiant FindById(Guid id);

    Task UpdateEtudiant(Etudiant etudiant);
    Etudiant? FindByHashCodePermanent(string hashCodePermanent);
    Task<Etudiant> AddEtudiantWIthoutSaveChanges(Etudiant etudiant);
    Task SaveChangesAsync();
}