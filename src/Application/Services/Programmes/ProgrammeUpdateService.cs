using Application.Exceptions.Programmes;
using Application.Interfaces.Services.Programmes;

using Domain.Entities.Programmes;
using Domain.Repositories;

namespace Application.Services.Programmes;

public class ProgrammeUpdateService(IProgrammeRepository programmeRepository) : IProgrammeUpdateService
{
    public async Task UpdateProgramme(Programme programme)
    {
        Programme? existingProgramme = programmeRepository.FindById(programme.Id);
        if (existingProgramme is null)
        {
            throw new ProgrammeNotFoundException($"Could not find programme with id {programme.Id}.");
        }

        await programmeRepository.UpdateProgramme(programme);
    }
}