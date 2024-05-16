using Domain.Entities.Programmes;

namespace Application.Interfaces.Services.Programmes;

public interface IProgrammeUpdateService
{
    Task UpdateProgramme(Programme programme);
}