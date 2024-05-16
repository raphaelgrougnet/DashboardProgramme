using Application.Exceptions.Programmes;
using Application.Interfaces;

using Domain.Entities.Programmes;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Programmes;

public class ProgrammeRepository(IDashboardProgrammeDbContext context) : IProgrammeRepository
{
    public async Task CreateProgramme(Programme programme)
    {
        if (context.Programmes.Any(x => x.Numero.Trim() == programme.Numero.Trim()))
        {
            throw new ProgrammeWithNumeroAlreadyExists($"An programme with numero {programme.Numero} already exists.");
        }

        context.Programmes.Add(programme);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProgrammeWithId(Guid id)
    {
        Programme? programme = context.Programmes.FirstOrDefault(x => x.Id == id);
        if (programme is null)
        {
            throw new ProgrammeNotFoundException($"Could not find programme with id {id}.");
        }

        context.Programmes.Remove(programme);
        await context.SaveChangesAsync();
    }

    public Programme FindById(Guid id)
    {
        Programme? programme = context.Programmes
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (programme is null)
        {
            throw new ProgrammeNotFoundException($"Could not find programme with id {id}.");
        }

        return programme;
    }

    public List<Programme> GetAll()
    {
        return context.Programmes.AsNoTracking().ToList();
    }

    public async Task<Programme?> FindByNumeroProgramme(string numeroProgramme)
    {
        Programme? programme = await context.Programmes
            .FirstOrDefaultAsync(x => x.Numero == numeroProgramme);

        if (programme is null)
        {
            throw new ProgrammeNotFoundException($"Could not find programme with numero {numeroProgramme}.");
        }

        return programme;
    }

    public async Task UpdateProgramme(Programme programme)
    {
        if (context.Programmes.Any(x => x.Numero == programme.Numero.Trim() && x.Id != programme.Id))
        {
            throw new ProgrammeWithNumeroAlreadyExists(
                $"Another programme with code permanent {programme.Numero} already exists.");
        }

        context.Programmes.Update(programme);
        await context.SaveChangesAsync();
    }
}