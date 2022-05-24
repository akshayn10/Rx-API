using Azure.Storage.Blobs;
using Rx.Domain.Interfaces.Blob;

namespace Rx.Infrastructure.Persistence;

public class BlobStorage:IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFile(FileStream stream, string productName)
    {
        var path = stream.Name;
        var extension = Path.GetExtension(path);
        var container = _blobServiceClient.GetBlobContainerClient("productlogo");
        var blob = container.GetBlobClient(productName + Guid.NewGuid().ToString("N") + extension);
        await blob.UploadAsync(stream);
        return blob.Uri.ToString();
    }
}