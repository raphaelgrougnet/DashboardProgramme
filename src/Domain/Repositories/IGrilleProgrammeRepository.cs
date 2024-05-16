using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Programmes;

namespace Domain.Repositories;

public interface IGrilleProgrammeRepository
{
    List<GrilleProgramme> GetAll();
    GrilleProgramme FindById(Guid id);
    Task<GrilleProgramme> AddGrilleProgrammeWithoutSaving(GrilleProgramme pGrilleProgramme);

    Task<GrilleProgramme?> FindByParams(Programme programme, byte etaleeSurNbSessions, ushort anneeMiseAJour);
    Task SaveChangesAsync();
}