using Application.Interfaces.FileStorage;

using Infrastructure.ExternalApis.Azure.Http;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.ExternalApis.Azure.Consumers;

public class AzureBlobApiConsumer(IAzureApiHttpClient apiClient) : IFileStorageApiConsumer
{
    private IAzureApiHttpClient ApiHttpClient { get; } = apiClient;

    public async Task DeleteFileWithUrl(string url)
    {
        await ApiHttpClient.DeleteFileAsync(url);
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        try
        {
            using MemoryStream ms = new();
            await file.CopyToAsync(ms);
            byte[] fileBytes = ms.ToArray();
            return await ApiHttpClient.UploadFileAsync(file.FileName, fileBytes, file.ContentType);
        }
        catch (Exception)
        {
            return string.Empty;
            // TODO : if you want to use Azure Blob Storage, uncomment the following line
            // throw new AzureApiException($"Invalid file, ${exception.Message}");
        }
    }
}