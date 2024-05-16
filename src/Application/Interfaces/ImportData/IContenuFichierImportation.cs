namespace Application.Interfaces.ImportData;

public interface IContenuFichierImportation
{
    List<IImportDataRow> Records { get; set; }
    string SheetName { get; set; }
    IDictionary<string, object> AdditionalProperties { get; set; }
}