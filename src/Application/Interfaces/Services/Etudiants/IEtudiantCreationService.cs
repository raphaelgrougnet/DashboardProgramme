using Domain.Entities.Etudiants;

namespace Application.Interfaces.Services.Etudiants;

public interface IEtudiantCreationService
{
    Task<Etudiant> CreateEtudiant(Etudiant etudiant);
    Etudiant FindByHashCodePermanent(string hashCodePermanent);
}