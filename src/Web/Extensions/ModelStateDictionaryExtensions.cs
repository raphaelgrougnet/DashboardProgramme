using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static string GetFirstErrorMessage(this ModelStateDictionary modelState)
    {
        List<ModelStateEntry> fieldsInError = modelState.Values.Where(x => x.Errors.Any()).ToList();
        return fieldsInError.Count != 0 ? fieldsInError.First().Errors.First().ErrorMessage : string.Empty;
    }
}