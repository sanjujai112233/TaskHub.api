using System;
using System.Reflection.Metadata;
using Azure.Storage.Blobs;

namespace TaskHub.api.Services;

public class AzureBlobService : IFileStorageSevice
{
    private readonly BlobContainerClient _container;

    public AzureBlobService(IConfiguration config)
    {
        var blobService = new BlobServiceClient(
            config["AzureBlob:ConnectionString"]
        );
        _container = blobService.GetBlobContainerClient(
            config["AzureBlob:Container"]
        );
    }

    public async Task<string> UploadAsync(IFormFile file)
    {

        var blobClient = _container.GetBlobClient(
            Guid.NewGuid() + Path.GetExtension(file.FileName)
        );

        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return blobClient.Uri.ToString();

    }


}
