using Application.Interfaces.Services.Programmes;

using Domain.Entities.Programmes;
using Domain.Repositories;

namespace Application.Services.Programmes;

public class ProgrammeCreationService(IProgrammeRepository programmeRepository) : IProgrammeCreationService
{
    public async Task CreateProgramme(Programme programme)
    {
        await programmeRepository.CreateProgramme(programme);
    }
}