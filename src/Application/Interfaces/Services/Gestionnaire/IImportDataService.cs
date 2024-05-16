using Application.Interfaces.ImportData;

namespace Application.Interfaces.Services.Gestionnaire;

public interface IImportDataService
{
    Task ImportData(IContenuFichierImportation contenuFichier);
    Task SaveDataInBD();
}