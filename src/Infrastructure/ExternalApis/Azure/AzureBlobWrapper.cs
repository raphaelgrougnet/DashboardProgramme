using Infrastructure.ExternalApis.Azure.Settings;

using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Infrastructure.ExternalApis.Azure;

public class AzureBlobWrapper(IOptions<AzureApiSettings> settings) : IAzureBlobWrapper
{
    private const string FOLDER_NAME = "images";

    public async Task<CloudBlockBlob> GetCloudBlockBlob(string fileName)
    {
        CloudStorageAccount? cloudStorageAccount = CloudStorageAccount.Parse(settings.Value.ConnectionString);
        CloudBlobClient? cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = await CreateCloudBlobContainer(cloudBlobClient, FOLDER_NAME);
        return cloudBlobContainer.GetBlockBlobReference(fileName);
    }

    private async Task<CloudBlobContainer> CreateCloudBlobContainer(CloudBlobClient cloudBlobClient,
        string containerName)
    {
        CloudBlobContainer? cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

        if (await cloudBlobContainer.CreateIfNotExistsAsync())
        {
            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        return cloudBlobContainer;
    }
}