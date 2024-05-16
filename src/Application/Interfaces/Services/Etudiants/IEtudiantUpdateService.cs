using Domain.Entities.Etudiants;

namespace Application.Interfaces.Services.Etudiants;

public interface IEtudiantUpdateService
{
    Task UpdateEtudiant(Etudiant etudiant);
}