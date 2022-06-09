using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Rx.Domain.Interfaces.Blob;

namespace Rx.Infrastructure.Persistence;

public class BlobStorage:IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFile(FileStream stream)
    {
        var path = stream.Name;
        var extension = Path.GetExtension(path);
        var container = _blobServiceClient.GetBlobContainerClient("productlogo");
        var blob = container.GetBlobClient("logo_"+Guid.NewGuid().ToString("N") + extension);
        await blob.UploadAsync(stream);
        return blob.Uri.ToString();
    }

    public async Task DeleteFile(string oldFileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient("productlogo");
        await container.DeleteBlobIfExistsAsync(oldFileName);
    }
}