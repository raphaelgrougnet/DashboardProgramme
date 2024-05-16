using Infrastructure.ExternalApis.Azure.Exceptions;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Infrastructure.ExternalApis.Azure.Http;

public class AzureApiHttpClient(IAzureBlobWrapper azureBlobWrapper) : IAzureApiHttpClient
{
    public async Task DeleteFileAsync(string fileName)
    {
        try
        {
            CloudBlockBlob cloudBlockBlob = await azureBlobWrapper.GetCloudBlockBlob(fileName);
            await cloudBlockBlob.DeleteIfExistsAsync();
        }
        catch (Exception exception)
        {
            throw new AzureApiException(exception.Message);
        }
    }

    public async Task<string> UploadFileAsync(string fileName, byte[] fileData, string fileMimeType)
    {
        try
        {
            fileName = $"{DateTime.Now.Ticks}-{fileName}";
            return await UploadToBlobAsync(fileName, fileData, fileMimeType);
        }
        catch (Exception exception)
        {
            throw new AzureApiException(exception.Message);
        }
    }

    private async Task<string> UploadToBlobAsync(string fileName, byte[] fileData, string fileMimeType)
    {
        CloudBlockBlob cloudBlockBlob = await azureBlobWrapper.GetCloudBlockBlob(fileName);
        cloudBlockBlob.Properties.ContentType = fileMimeType;
        await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
        return cloudBlockBlob.Uri.AbsoluteUri;
    }
}