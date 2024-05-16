using Domain.Entities.Programmes;

namespace Application.Interfaces.Services.Programmes;

public interface IProgrammeCreationService
{
    Task CreateProgramme(Programme programme);
}