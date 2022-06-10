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

    public async Task<string> UploadLogo(FileStream stream)
    {
        var path = stream.Name;
        var extension = Path.GetExtension(path);
        var container = _blobServiceClient.GetBlobContainerClient("productlogo");
        var blob = container.GetBlobClient("logo_"+Guid.NewGuid().ToString("N") + extension);
        stream.Position = 0;
        await blob.UploadAsync(stream);
        return blob.Uri.ToString();
    }
    public async Task<string> UploadProfile(FileStream stream)
    {
        var path = stream.Name;
        var extension = Path.GetExtension(path);
        var container = _blobServiceClient.GetBlobContainerClient("profileimages");
        var blob = container.GetBlobClient("profile_"+Guid.NewGuid().ToString("N") + extension);
        stream.Position = 0;
        await blob.UploadAsync(stream);
        return blob.Uri.ToString();
    }

    public async Task DeleteFile(string oldFileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient("productlogo");
        await container.DeleteBlobIfExistsAsync(oldFileName);
    }
}