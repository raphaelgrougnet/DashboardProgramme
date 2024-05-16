using Application.Interfaces.Services.Gestionnaire;

using FastEndpoints;

using FluentValidation.Results;

using LateApexEarlySpeed.Json.Schema;

using Microsoft.AspNetCore.Authentication.Cookies;

using Newtonsoft.Json;

using Web.Features.Common;

using ValidationResult = LateApexEarlySpeed.Json.Schema.Common.ValidationResult;

namespace Web.Features.Gestionnaire.ImportData;

public class ImportDataEndpoint : Endpoint<ImportDataRequest, SucceededOrNotResponse>
{
    private readonly IImportDataService _importDataService;
    private readonly JsonValidator _jsonValidator = null!;

    public ImportDataEndpoint(IImportDataService importDataService)
    {
        _importDataService = importDataService;
        string directoryPath = Directory.GetCurrentDirectory();
        string cheminFichier = Path.Combine(directoryPath, @"Features\Gestionnaire\ImportData\schema-importation.json");
        string jsonSchema = File.ReadAllText(cheminFichier);
        _jsonValidator = new JsonValidator(jsonSchema);
    }

    public override void Configure()
    {
        DontCatchExceptions();
        Post("importData");
        //Roles(Domain.Constants.User.Roles.ADMINISTRATOR,Domain.Constants.User.Roles.Gestionnaire);
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }


    public override async Task HandleAsync(ImportDataRequest reqWrapper, CancellationToken ct)
    {
        string req = reqWrapper.Content;
        ValidationResult validationResult = _jsonValidator.Validate(req);

        if (!validationResult.IsValid)
        {
            ValidationFailures.Add(new ValidationFailure("json", validationResult.ErrorMessage));
            await SendErrorsAsync(400, ct);
            return;
        }

        ContenuFichierImportation? contenuFichier;
        try
        {
            contenuFichier = JsonConvert.DeserializeObject<ContenuFichierImportation>(req);
        }
        catch (Exception e)
        {
            ValidationFailures.Add(new ValidationFailure("json", e.Message ?? "Le fichier JSON est invalide"));
            await SendErrorsAsync(400, ct);
            return;
        }

        if (contenuFichier == null)
        {
            ValidationFailures.Add(new ValidationFailure("json", "Le fichier JSON est invalide"));
            await SendErrorsAsync(400, ct);
            return;
        }

        await _importDataService.ImportData(contenuFichier);
        await SendOkAsync(ct);
    }
}