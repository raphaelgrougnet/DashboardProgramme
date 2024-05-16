namespace Infrastructure.ExternalApis.Azure.Exceptions;

public class AzureApiException(string message) : Exception(message);