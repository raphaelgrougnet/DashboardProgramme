using FastEndpoints;

namespace Web.Features.Gestionnaire.ImportData;

public class ImportDataRequest : IPlainTextRequest
{
    public string Content { get; set; }
}