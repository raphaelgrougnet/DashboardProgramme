namespace Domain.Repositories;

public interface IImportDataRepository
{
    Task ImportData(List<Dictionary<string, object>> record, string sheetName);
}