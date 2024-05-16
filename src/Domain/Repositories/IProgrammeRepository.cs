using Domain.Entities.Programmes;

namespace Domain.Repositories;

public interface IProgrammeRepository
{
    List<Programme> GetAll();
    Programme FindById(Guid id);
    Task CreateProgramme(Programme programme);
    Task UpdateProgramme(Programme programme);
    Task DeleteProgrammeWithId(Guid id);
    Task<Programme?> FindByNumeroProgramme(string numeroProgramme);
}